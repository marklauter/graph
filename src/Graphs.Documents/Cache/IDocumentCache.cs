using System;
using System.Collections.Generic;

namespace Graphs.Documents
{

    public interface IDocumentCache<T>
        where T : class
    {
        event EventHandler<CacheAccessedEventArgs> CacheAccessed;
        event EventHandler<CacheItemEvictedEventArgs<T>> CacheItemEvicted;

        void Clear();

        void Evict(string key);
        void Evict(Document<T> document);

        Document<T> Read(string key, Func<string, Document<T>> itemFactory);

        IEnumerable<Document<T>> Read(IEnumerable<string> keys, Func<string, Document<T>> itemFactory);
    }
}
