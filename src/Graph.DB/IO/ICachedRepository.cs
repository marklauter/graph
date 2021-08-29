using Graphs.DB.Elements;
using System;

namespace Graphs.DB.IO
{
    internal interface ICachedRepository<T>
        : IRepository<T>
        where T : IElement

    {
        event EventHandler<KeyEventArgs> CacheHit;
        event EventHandler<KeyEventArgs> CacheMiss;
        event EventHandler<CacheItemEvictedEventArgs<T>> CacheItemEvicted;
    }
}
