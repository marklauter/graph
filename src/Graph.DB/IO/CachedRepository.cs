using Graphs.DB.Elements;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs.DB.IO
{
    public sealed class CachedRepository<T>
        : Repository<T>
        , ICachedRepository<T>
        , IDisposable
        where T : IElement
    {
        // tood: implement reader writer locker slim
        private readonly HashSet<string> keys = new();
        private readonly IMemoryCache cache;
        private readonly IRepository<T> repository;
        private readonly MemoryCacheEntryOptions cacheEntryOptions;
        private bool disposedValue;

        public event EventHandler<KeyEventArgs> CacheHit;
        public event EventHandler<KeyEventArgs> CacheMiss;
        public event EventHandler<CacheItemEvictedEventArgs<T>> CacheItemEvicted;

        public CachedRepository(IRepository<T> repository, MemoryCacheEntryOptions cacheEntryOptions)
            : base()
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.repository.Deleted += this.Repository_Deleted;
            this.repository.Updated += this.Repository_Updated;

            var cacheOptions = new MemoryCacheOptions();
            this.cache = new MemoryCache(cacheOptions);

            this.cacheEntryOptions = cacheEntryOptions ?? throw new ArgumentNullException(nameof(cacheEntryOptions));
            this.cacheEntryOptions.RegisterPostEvictionCallback(this.OnPostEviction);
        }

        public CachedRepository(IRepository<T> repository)
            : this(repository, new MemoryCacheEntryOptions())
        {
        }

        public override int Count()
        {
            return this.repository.Count();
        }

        public override int Delete(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            this.cache.Remove(key);
            this.keys.Remove(key);
            var i = this.repository.Delete(key);
            this.OnDeleted(new KeyEventArgs(key));
            return i;
        }

        public override IEnumerable<Entity<T>> Entities()
        {
            foreach (var key in this.keys)
            {
                if (this.cache.TryGetValue<Entity<T>>(key, out var entity))
                {
                    yield return entity;
                }
            }

            foreach (var entity in this.repository.Entities(this.keys))
            {
                yield return entity;
            }
        }

        public override IEnumerable<Entity<T>> Entities(IEnumerable<string> excludedKeys)
        {
            CheckKeys(excludedKeys);

            var includedKeys = this.keys.Except(excludedKeys);
            foreach (var key in includedKeys)
            {
                if (this.cache.TryGetValue<Entity<T>>(key, out var entity))
                {
                    yield return entity;
                }
            }

            foreach (var entity in this.repository.Entities(excludedKeys.Union(includedKeys)))
            {
                yield return entity;
            }
        }

        public override Entity<T> Insert(T element)
        {
            var entity = this.cache.Set(element.Key, this.repository.Insert(element), this.cacheEntryOptions);
            this.keys.Add(element.Key);
            this.OnInserted(new EntityEventArgs<T>(entity));
            return entity;
        }

        public override Entity<T> Select(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            if (this.cache.TryGetValue<Entity<T>>(key, out var entity))
            {
                this.CacheHit?.Invoke(this, new KeyEventArgs(key));
            }
            else
            {
                entity = this.repository.Select(key);
                this.cache.Set(key, entity, this.cacheEntryOptions);
                this.keys.Add(key);
                this.CacheMiss?.Invoke(this, new KeyEventArgs(key));
            }

            this.OnSelected(new EntityEventArgs<T>(entity));
            return entity;
        }

        public override IEnumerable<Entity<T>> Select(Func<T, bool> predicate)
        {
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var elements = this.Entities()
                .Select(entity => entity.Member)
                .Where(predicate)
                .Select(element =>
                {
                    var entity = (Entity<T>)element;
                    this.OnSelected(new EntityEventArgs<T>(entity));
                    return entity;
                });

            var uncachedKeys = elements
                .Where(entity => !this.keys.Contains(entity.Key))
                .Select(entity => this.cache.Set(entity.Key, entity, this.cacheEntryOptions))
                .Select(entity => entity.Key);

            var cachedKeys = elements
                .Where(entity => this.keys.Contains(entity.Key))
                .Select(entity => entity.Key);

            this.keys.UnionWith(uncachedKeys);

            foreach (var key in cachedKeys)
            {
                this.CacheHit?.Invoke(this, new KeyEventArgs(key));
            }

            foreach (var key in uncachedKeys)
            {
                this.CacheMiss?.Invoke(this, new KeyEventArgs(key));
            }

            return elements;
        }

        public override int Update(Entity<T> entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            this.keys.Add(entity.Key);
            this.cache.Set(entity.Key, entity, this.cacheEntryOptions);
            var i = this.repository.Update(entity);
            this.OnUpdated(new EntityEventArgs<T>(entity));
            return i;
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

        private static void CheckKeys(IEnumerable<string> excludedKeys)
        {
            if (excludedKeys is null)
            {
                throw new ArgumentNullException(nameof(excludedKeys));
            }
        }

        private void Repository_Updated(object sender, EntityEventArgs<T> e)
        {
            if (this.keys.Contains(e.Entity.Key))
            {
                this.cache.Remove(e.Entity.Key);
                this.keys.Remove(e.Entity.Key);
                this.OnDeleted(new KeyEventArgs(e.Entity.Key));
            }
        }

        private void Repository_Deleted(object sender, KeyEventArgs e)
        {
            if (this.keys.Contains(e.Key))
            {
                this.cache.Remove(e.Key);
                this.keys.Remove(e.Key);
                this.OnDeleted(new KeyEventArgs(e.Key));
            }
        }

        private void OnPostEviction(object key, object value, EvictionReason reason, object state)
        {
            this.keys.Remove(key as string);
            this.CacheItemEvicted?.Invoke(this, new CacheItemEvictedEventArgs<T>(value as Entity<T>, reason));
        }
    }
}
