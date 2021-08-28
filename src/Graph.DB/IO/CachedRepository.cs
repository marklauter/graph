using Graphs.DB.Elements;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs.DB.IO
{
    // HostFileChangeMonitor - https://docs.microsoft.com/en-us/dotnet/api/system.runtime.caching.hostfilechangemonitor?view=dotnet-plat-ext-5.0
    // FileChangeMonitor  - https://docs.microsoft.com/en-us/dotnet/api/system.runtime.caching.filechangemonitor?view=dotnet-plat-ext-5.0
    // LazyCache - https://blog.novanet.no/asp-net-core-memory-cache-is-get-or-create-thread-safe/

    public class CachedRepository<T>
        : Repository<T>
        , IRepository<T>
        , IDisposable
        where T : IElement
    {
        // todo: remove keys on expiration
        // tood: implement reader writer locker slim
        private readonly HashSet<string> keys = new();
        private readonly IMemoryCache cache;
        private readonly IRepository<T> repository;
        private readonly MemoryCacheEntryOptions cacheEntryOptions;
        private bool disposedValue;

        public CachedRepository(IRepository<T> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            var cacheOptions = new MemoryCacheOptions();
            this.cache = new MemoryCache(cacheOptions);
        }

        public CachedRepository(IRepository<T> repository, MemoryCacheEntryOptions cacheEntryOptions)
            : this(repository)
        {
            this.cacheEntryOptions = cacheEntryOptions ?? throw new ArgumentNullException(nameof(cacheEntryOptions));
        }

        public override int Count()
        {
            return this.repository.Count();
        }

        public override int Delete(string key)
        {
            this.cache.Remove(key);
            return this.repository.Delete(key);
        }

        public override IEnumerable<Entity<T>> Entities()
        {
            foreach (var key in this.keys)
            {
                if (this.cache.TryGetValue(key, out var entity))
                {
                    yield return (Entity<T>)entity;
                }
            }

            foreach (var entity in this.repository.Entities(this.keys))
            {
                yield return entity;
            }
        }

        public override IEnumerable<Entity<T>> Entities(IEnumerable<string> excludedKeys)
        {
            var includedKeys = this.keys.Except(excludedKeys);
            foreach (var key in includedKeys)
            {
                if (this.cache.TryGetValue(key, out var entity))
                {
                    yield return (Entity<T>)entity;
                }
            }

            foreach (var entity in this.repository.Entities(excludedKeys.Union(includedKeys)))
            {
                yield return entity;
            }
        }

        public override Entity<T> Insert(T element)
        {
            _ = this.keys.Add(element.Key);
            return this.cache.Set(element.Key, this.repository.Insert(element));
        }

        public override Entity<T> Read(string key)
        {
            return this.cache
                .GetOrCreate(key, cacheEntry =>
                {
                    _ = this.cacheEntryOptions != null
                        ? cacheEntry.SetOptions(this.cacheEntryOptions)
                        : null;

                    _ = this.keys.Add(key);

                    return this.repository.Read(key);
                });
        }

        public override IEnumerable<Entity<T>> Read(Func<T, bool> predicate)
        {
            var elements = (this as IRepository<T>).Entities()
                .Select(entity => entity.Member)
                .Where(predicate)
                .Select(element => (Entity<T>)element);

            var uncachedKeys = elements
                .Where(entity => !this.keys.Contains(entity.Key))
                .Select(entity => this.cache.Set(entity.Key, entity))
                .Select(entity => entity.Key);

            this.keys.UnionWith(uncachedKeys);

            return elements;
        }

        public override int Update(Entity<T> entity)
        {
            this.cache.Set(entity.Key, entity);
            return this.repository.Update(entity);
        }

        protected virtual void Dispose(bool disposing)
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

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
