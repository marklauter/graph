using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Graphs.Documents.IO
{
    public abstract class PersistentDocumentCollection<T>
        : DocumentCollection<T>
        , IDisposable
        where T : class
    {
        // queues are better than locks, but still need to implement locking waits for file open
        private readonly ConcurrentQueue<DocumentActionItem> actionQueue = new();
        private readonly IDocumentSerializer<T> serializer;
        private readonly string path;
        private readonly TimeSpan fileLockTimeout;
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private bool disposedValue;

        protected PersistentDocumentCollection(
            IDocumentSerializer<T> serializer,
            string path,
            TimeSpan fileLockTimeout)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }

            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.path = path;
            this.fileLockTimeout = fileLockTimeout;

            this.StartActionQueueProcessor();
        }

        public override int Count => Directory.EnumerateFiles(this.path).Count();

        public override bool Contains(string key)
        {
            return File.Exists(this.GetFileName(key));
        }

        public void Flush()
        {
            while (this.actionQueue.TryDequeue(out var actionItem))
            {
                this.ProcessActionItem(actionItem);
            }
        }

        public override IEnumerator<Document<T>> GetEnumerator()
        {
            foreach (var file in Directory.EnumerateFiles(this.path))
            {
                yield return this.ReadFile(file);
            }
        }

        protected override void AddDocument(Document<T> document)
        {
            this.actionQueue.Enqueue(new DocumentActionItem(document, DocumentAction.Add));
        }

        protected override void ClearCollection()
        {
            foreach (var file in Directory.EnumerateFiles(this.path))
            {
                this.DeleteFile(file);
            }
        }

        protected override Document<T> ReadDocument(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            return this.ReadFile(this.GetFileName(key));
        }

        protected override void RemoveDocument(string key)
        {
            this.actionQueue.Enqueue(new DocumentActionItem(key, DocumentAction.Remove));
        }

        protected override void UpdateDocument(Document<T> document)
        {
            this.actionQueue.Enqueue(new DocumentActionItem(document, DocumentAction.Update));
        }

        private void StartActionQueueProcessor()
        {
            // todo: https://www.minatcoding.com/blog/tech-tips/tech-tip-creating-a-long-running-background-task-in-net-core/
            this.ProcessActionQueue(this.cancellationTokenSource.Token);
        }

        private void ProcessActionQueue(CancellationToken cancellationToken)
        {
            var wait = new SpinWait();
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!this.actionQueue.TryDequeue(out var actionItem))
                {
                    wait.SpinOnce();
                    continue;
                }

                this.ProcessActionItem(actionItem);
            }

            this.Flush();
        }

        private void ProcessActionItem(DocumentActionItem actionItem)
        {
            switch (actionItem.Action)
            {
                case DocumentAction.Add:
                case DocumentAction.Update:
                    var document = actionItem.Item as Document<T>;
                    this.WriteFile(this.GetFileName(document.Key), document);
                    break;
                case DocumentAction.Remove:
                    var key = actionItem.Item as string;
                    this.DeleteFile(this.GetFileName(key));
                    break;
            }
        }

        private void DeleteFile(string filename)
        {
            ThreadSafeFile.Delete(filename, this.fileLockTimeout);
        }

        private Document<T> ReadFile(string fileName)
        {
            using var stream = ThreadSafeFile.Open(
                fileName,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                this.fileLockTimeout);
            return this.serializer.Deserialize(stream);
        }

        private void WriteFile(string fileName, Document<T> document)
        {
            using var stream = ThreadSafeFile.Open(
                fileName,
                FileMode.OpenOrCreate,
                FileAccess.Write,
                FileShare.None,
                this.fileLockTimeout);
            this.serializer.Serialize(document, stream);
            stream.Flush();
        }

        private string GetFileName(string key)
        {
            return Path.Combine(this.path, $"{key}.{DocumentKeys<T>.TypeName}");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.cancellationTokenSource.Cancel();
                }

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
