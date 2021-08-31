using Microsoft.Extensions.Caching.Memory;
using Repositories.Locking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
    public sealed class Cache<T>
        : ICache<T>
        , IDisposable
        where T : class
    {
        private bool disposedValue;
        private readonly IMemoryCache cache;
        private readonly IRepository<T> repository;
        private readonly TimeSpan lockTimeout;
        private readonly MemoryCacheEntryOptions cacheEntryOptions;
        private readonly NamedLocks locks = new();

        public event EventHandler<CacheAccessedArgs> CacheAccessed;
        public event EventHandler<CacheItemEvictedEventArgs<T>> CacheItemEvicted;

        public Cache(IRepository<T> repository, TimeSpan lockTimeout)
            : this(repository, lockTimeout, new MemoryCacheEntryOptions())
        {

        }

        public Cache(IRepository<T> repository, TimeSpan lockTimeout, MemoryCacheEntryOptions cacheEntryOptions)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.lockTimeout = lockTimeout;
            this.repository.Deleted += this.Repository_Deleted;
            this.repository.Updated += this.Repository_Updated;

            this.cache = new MemoryCache(new MemoryCacheOptions());

            this.cacheEntryOptions = cacheEntryOptions ?? throw new ArgumentNullException(nameof(cacheEntryOptions));
            this.cacheEntryOptions.RegisterPostEvictionCallback(this.OnPostEviction);
        }

        public Entity<T> Read(string key)
        {
            var accessType = CacheAccessType.Hit;
            Entity<T> entity;
            var gate = this.locks.EnterUpgradeableReadLock(key, this.lockTimeout);
            try
            {
                if (!this.cache.TryGetValue(key, out entity))
                {
                    gate.TryEnterWriteLock(this.lockTimeout);
                    try
                    {
                        entity = this.cache.Set(
                            key, 
                            this.repository.Select(key), 
                            this.cacheEntryOptions);
                        accessType = CacheAccessType.Miss;
                    }
                    finally
                    {
                        gate.ExitWriteLock();
                    }
                }
            }
            finally
            {
                this.locks.ExitUpgradeableReadLock(gate);
            }

            this.CacheAccessed?.Invoke(this, new CacheAccessedArgs(key, accessType));

            return entity;
        }

        public IEnumerable<Entity<T>> Read(IEnumerable<string> keys)
        {
            return keys.Select(key => this.Read(key));
        }

        private void Repository_Updated(object sender, EntityEventArgs<T> e)
        {
            this.cache.Remove(e.Entity.Key);
        }

        private void Repository_Deleted(object sender, KeyEventArgs e)
        {
            this.cache.Remove(e.Key);
        }

        private void OnPostEviction(object key, object value, EvictionReason reason, object state)
        {
            this.CacheItemEvicted?.Invoke(this, new CacheItemEvictedEventArgs<T>(value as Entity<T>, reason));
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.cache.Dispose();
                }

                this.disposedValue = true;
            }
        }
    }
}
