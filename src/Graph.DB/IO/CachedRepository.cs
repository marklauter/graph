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
        : IRepository<T>
        where T : IElement
    {
        // todo: remove keys on expiration
        private readonly HashSet<string> keys = new();
        private readonly IMemoryCache cache;
        private readonly IRepository<T> repository;
        private readonly MemoryCacheEntryOptions cacheEntryOptions;

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

        public int Delete(string key)
        {
            return this.repository.Delete(key);
        }

        public int Delete(Entity<T> entity)
        {
            return this.repository.Delete(entity);
        }

        public int Delete(T element)
        {
            return this.repository.Delete(element);
        }

        public int Delete(IEnumerable<Entity<T>> entities)
        {
            return this.repository.Delete(entities);
        }

        public int Delete(IEnumerable<T> elements)
        {
            return this.repository.Delete(elements);
        }

        public int Delete(Func<T, bool> predicate)
        {
            return this.repository.Delete(predicate);
        }

        public Entity<T> Insert(T element)
        {
            _ = this.keys.Add(element.Key);
            return this.cache.Set(element.Key, this.repository.Insert(element));
        }

        public IEnumerable<Entity<T>> Insert(IEnumerable<T> elements)
        {
            return elements == null
                ? throw new ArgumentNullException(nameof(elements))
                : elements.Select(model => this.Insert(model));
        }

        public Entity<T> Read(string key)
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

        public IEnumerable<Entity<T>> Read(IEnumerable<string> keys)
        {
            return keys.Select(key => this.Read(key));
        }

        public IEnumerable<Entity<T>> Read(Func<T, bool> predicate)
        {
            var elements = this.EnumerateEntities()
                .Select(e => e.Member)
                .Where(predicate);



            return this.repository.Read(predicate);
        }

        public int Update(Entity<T> entity)
        {
            return this.repository.Update(entity);
        }

        public int Update(T element)
        {
            return this.repository.Update(element);
        }

        public int Update(IEnumerable<Entity<T>> entities)
        {
            return this.repository.Update(entities);
        }

        public int Update(IEnumerable<T> elements)
        {
            return this.repository.Update(elements);
        }

        private IEnumerable<Entity<T>> EnumerateEntities()
        {
            foreach (var key in this.keys)
            {
                if (this.cache.TryGetValue(key, out var entity))
                {
                    yield return (Entity<T>)entity;
                }
            }
        }
    }
}
