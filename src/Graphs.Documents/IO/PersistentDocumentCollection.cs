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
        private readonly ConcurrentQueue<Document<T>> addQueue = new();
        private readonly ConcurrentQueue<string> removeQueue = new();
        private readonly ConcurrentQueue<Document<T>> updateQueue = new();
        private readonly string path;
        private readonly TimeSpan lockTimeout;

        public PersistentDocumentCollection(string path, TimeSpan lockTimeout)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }

            this.path = path;
            this.lockTimeout = lockTimeout;
        }

        public override int Count => Directory.EnumerateFiles(this.path).Count();

        public override bool Contains(string key)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        protected override void AddDocument(Document<T> document)
        {
            this.addQueue.Enqueue(document);
        }

        protected override void ClearCollection()
        {
            throw new NotImplementedException();
        }

        protected override Document<T> ReadDocument(string key)
        {
            throw new NotImplementedException();
        }

        protected override void RemoveDocument(string key)
        {
            this.removeQueue.Enqueue(key);
        }

        protected override void UpdateDocument(Document<T> document)
        {
            this.updateQueue.Enqueue(document);
        }

        protected abstract Document<T> DeserializeDocument(Stream stream);

        protected abstract void SerializeDocument(Document<T> document);
    }
}
