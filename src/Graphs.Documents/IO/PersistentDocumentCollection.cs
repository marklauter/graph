using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graphs.Documents.IO
{
    public abstract class PersistentDocumentCollection<T>
        : DocumentCollection<T>
        , IDisposable
        where T : class
    {
        // todo: add cache for reads
        private readonly string path;
        private readonly TimeSpan fileLockTimeout;
        private readonly IDocumentSerializer<T> serializer;
        private readonly DocumentActionQueueProcessor<T> actionQueue;
        private bool disposedValue;

        protected PersistentDocumentCollection(
            string path,
            TimeSpan fileLockTimeout)
            : this(path, fileLockTimeout, null)
        {

        }

        protected PersistentDocumentCollection(
            string path,
            TimeSpan fileLockTimeout,
            IDocumentSerializer<T> serializer)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }

            this.serializer = serializer ?? new JsonDocumentSerializer<T>();
            this.actionQueue = new DocumentActionQueueProcessor<T>(this.DeleteFile, this.WriteFile);
            this.fileLockTimeout = fileLockTimeout;
            this.path = path;
        }

        public override int Count => Directory.EnumerateFiles(this.path).Count();

        public override bool Contains(string key)
        {
            return File.Exists(this.GetFileName(key));
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
            this.actionQueue.EnqueueAddAction(document);
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
            return String.IsNullOrWhiteSpace(key)
                ? throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key))
                : this.ReadFile(this.GetFileName(key));
        }

        protected override void RemoveDocument(string key)
        {
            this.actionQueue.EnqueueRemoveAction(key);
        }

        protected override void UpdateDocument(Document<T> document)
        {
            this.actionQueue.EnqueueUpdateAction(document);
        }

        private void DeleteFile(string key)
        {
            ThreadSafeFile.Delete(this.GetFileName(key), this.fileLockTimeout);
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

        private void WriteFile(Document<T> document)
        {
            using var stream = ThreadSafeFile.Open(
                this.GetFileName(document.Key),
                FileMode.OpenOrCreate,
                FileAccess.Write,
                FileShare.None,
                this.fileLockTimeout);
            this.serializer.Serialize(document, stream);
            stream.Flush(true);
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
                    this.actionQueue.Dispose();
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
