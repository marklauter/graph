using System;
using System.Collections.Generic;

namespace Graphs.Documents
{

    // todo: best advice to avoiding the need for locking is to use a concurrent queue for writes
    public interface IDocumentCollection<T>
        : IEnumerable<T>
        where T : class
    {
        event EventHandler<DocumentAddedEventArgs<T>> DocumentAdded;
        event EventHandler<DocumentRemovedEventArgs<T>> DocumentRemoved;
        event EventHandler<DocumentUpdatedEventArgs<T>> DocumentUpdated;

        event EventHandler<EventArgs> Cleared;

        int Count { get; }

        void Add(Document<T> document);
        void Add(IEnumerable<Document<T>> documents);

        void Clear();

        bool Contains(string key);

        Document<T> Read(string key);
        IEnumerable<Document<T>> Read(IEnumerable<string> keys);

        void Remove(string key);
        void Remove(IEnumerable<string> keys);

        void Remove(Document<T> document);
        void Remove(IEnumerable<Document<T>> documents);

        void Update(Document<T> document);
        void Update(IEnumerable<Document<T>> documents);
    }
}
