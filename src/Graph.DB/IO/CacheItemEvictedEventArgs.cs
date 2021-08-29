using Graphs.DB.Elements;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Graphs.DB.IO
{
    public class CacheItemEvictedEventArgs<T>
        : EventArgs
        where T : IElement
    {
        public CacheItemEvictedEventArgs(Entity<T> entity, EvictionReason reason)
        {
            this.Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            this.Reason = reason;
        }

        public Entity<T> Entity { get; }
        public EvictionReason Reason { get; }
    }
}
