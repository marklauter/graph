using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graphs.Documents.IO
{
    public abstract class PersistentDocumentCollection<T>
        : DocumentCollection<T>
        where T : class
    {
        // queues are better than locks, but still need to implement locking waits for file open
        private readonly ConcurrentQueue<DocumentActionItem> actionQueue = new();
        private readonly IDocumentSerializer<T> serializer;
        private readonly string path;
        private readonly TimeSpan lockTimeout;

        protected PersistentDocumentCollection(
            IDocumentSerializer<T> serializer,
            string path,
            TimeSpan lockTimeout)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }

            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.path = path;
            this.lockTimeout = lockTimeout;
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
            this.actionQueue.Enqueue(new DocumentActionItem(document, DocumentAction.Add));
        }

        protected override void ClearCollection()
        {
            foreach (var file in Directory.EnumerateFiles(this.path))
            {
                ThreadSafeFile.Delete(file, this.lockTimeout);
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

        private Document<T> ReadFile(string fileName)
        {
            using var stream = ThreadSafeFile.Open(
                fileName, 
                FileMode.Open, 
                FileAccess.Read, 
                FileShare.Read, 
                this.lockTimeout);
            return this.serializer.Deserialize(stream);
        }

        private void WriteFile(string fileName, Document<T> document)
        {
            using var stream = ThreadSafeFile.Open(
                fileName, 
                FileMode.OpenOrCreate, 
                FileAccess.Write, 
                FileShare.None, 
                this.lockTimeout);
            this.serializer.Serialize(document, stream);
            stream.Flush();
        }

        private string GetFileName(string key)
        {
            return Path.Combine(this.path, $"{key}.{DocumentKeys<T>.TypeName}");
        }
    }
}
