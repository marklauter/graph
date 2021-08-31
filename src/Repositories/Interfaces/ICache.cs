using System;
using System.Collections.Generic;

namespace Repositories
{
    public interface ICache<T>
        where T : class
    {
        event EventHandler<KeyEventArgs> CacheHit;
        event EventHandler<KeyEventArgs> CacheMiss;
        event EventHandler<CacheItemEvictedEventArgs<T>> CacheItemEvicted;

        Entity<T> Read(string key);

        IEnumerable<Entity<T>> Read(IEnumerable<string> keys);
    }
}
