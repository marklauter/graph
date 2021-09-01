using System;
using System.Collections.Generic;

namespace Graphs.Documents
{
    public interface IDocumentCache<T>
        where T : class
    {
        event EventHandler<CacheReadEventArgs> CacheAccessed;
        event EventHandler<CacheItemEvictedEventArgs<T>> CacheItemEvicted;

        void Evict(Document<T> document);

        Document<T> Read(string key);

        IEnumerable<Document<T>> Read(IEnumerable<string> keys);
    }
}
