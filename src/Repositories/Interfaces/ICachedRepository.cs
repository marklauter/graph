using System;

namespace Repositories
{
    public interface ICachedRepository<T>
        : IRepository<T>
        where T : class

    {
        event EventHandler<KeyEventArgs> CacheHit;
        event EventHandler<KeyEventArgs> CacheMiss;
        event EventHandler<CacheItemEvictedEventArgs<T>> CacheItemEvicted;
    }
}
