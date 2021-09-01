using System;
using System.Collections.Generic;

namespace Graphs.Documents
{
    public interface IDocumentCollection<T>
        : IEnumerable<T>
        where T : class
    {
        event EventHandler<DocumentRemovedEventArgs> Removed;
        event EventHandler<DocumentAddedEventArgs<T>> Added;
        event EventHandler<DocumentUpdatedEventArgs<T>> Updated;

        int Count { get; }

        Document<T> Add(Document<T> document);
        IEnumerable<Document<T>> Add(IEnumerable<Document<T>> documents);

        void Clear();

        bool Contains(string key);

        Document<T> Read(string key);
        IEnumerable<Document<T>> Read(IEnumerable<string> keys);

        bool Remove(string key);
        int Remove(IEnumerable<string> keys);

        bool Remove(Document<T> document);
        int Remove(IEnumerable<Document<T>> documents);

        Document<T> Update(Document<T> document);
        IEnumerable<Document<T>> Update(IEnumerable<Document<T>> documents);
    }
}
