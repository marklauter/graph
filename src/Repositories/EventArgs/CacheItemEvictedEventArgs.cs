using Microsoft.Extensions.Caching.Memory;
using System;

namespace Repositories
{
    public class CacheItemEvictedEventArgs<T>
        : EventArgs
        where T : class
    {
        public CacheItemEvictedEventArgs(Entity<T> item, EvictionReason reason)
        {
            this.Item = item ?? throw new ArgumentNullException(nameof(item));
            this.Reason = reason;
        }

        public Entity<T> Item { get; }

        public EvictionReason Reason { get; }
    }
}
