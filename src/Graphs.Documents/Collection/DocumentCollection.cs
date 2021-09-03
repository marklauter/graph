using System;
using System.Collections;
using System.Collections.Generic;

namespace Graphs.Documents
{
    public abstract class DocumentCollection<T>
        : IDocumentCollection<T>
        where T : class
    {
        public event EventHandler<DocumentAddedEventArgs<T>> DocumentAdded;
        public event EventHandler<DocumentRemovedEventArgs<T>> DocumentRemoved;
        public event EventHandler<DocumentUpdatedEventArgs<T>> DocumentUpdated;

        public event EventHandler<EventArgs> Cleared;

        public abstract int Count { get; }

        public void Add(Document<T> document)
        {
            if (document is null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            this.AddDocument(document);

            this.DocumentAdded?.Invoke(this, new DocumentAddedEventArgs<T>(document));
        }

        public void Add(IEnumerable<Document<T>> documents)
        {
            if (documents is null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            foreach (var document in documents)
            {
                this.Add(document);
            }
        }

        public void Clear()
        {
            this.ClearCollection();
            this.Cleared?.Invoke(this, EventArgs.Empty);
        }

        public abstract bool Contains(string key);

        public abstract IEnumerator<Document<T>> GetEnumerator();

        public Document<T> Read(string key)
        {
            return String.IsNullOrWhiteSpace(key)
                ? throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key))
                : this.ReadDocument(key);
        }

        public IEnumerable<Document<T>> Read(IEnumerable<string> keys)
        {
            return keys is null
                ? throw new ArgumentNullException(nameof(keys))
                : this.ReadDocuments(keys);
        }

        public void Remove(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            var document = this.Read(key);
            this.RemoveDocument(key);
            this.DocumentRemoved?.Invoke(this, new DocumentRemovedEventArgs<T>(document));
        }

        public void Remove(IEnumerable<string> keys)
        {
            if (keys is null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            foreach (var key in keys)
            {
                this.Remove(key);
            }
        }

        public void Remove(Document<T> document)
        {
            if (document is null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            this.Remove(document.Key);
        }

        public void Remove(IEnumerable<Document<T>> documents)
        {
            if (documents is null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            foreach (var document in documents)
            {
                this.Remove(document);
            }
        }

        public void Update(Document<T> document)
        {
            if (document is null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var d = this.Read(document.Key);
            if (d.ETag != document.ETag)
            {
                throw new ETagMismatchException($"key: {document.Key}, expected: {d.ETag}, actual: {document.ETag}");
            }

            this.UpdateDocument(document);

            this.DocumentUpdated?.Invoke(this, new DocumentUpdatedEventArgs<T>(document));
        }

        public void Update(IEnumerable<Document<T>> documents)
        {
            if (documents is null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            foreach (var document in documents)
            {
                this.Update(document);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected abstract void AddDocument(Document<T> document);

        protected abstract void ClearCollection();

        protected abstract Document<T> ReadDocument(string key);

        protected abstract void RemoveDocument(string key);

        protected abstract void UpdateDocument(Document<T> document);

        private IEnumerable<Document<T>> ReadDocuments(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                yield return this.Read(key);
            }
        }
    }
}
