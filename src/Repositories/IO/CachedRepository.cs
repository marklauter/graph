//using Microsoft.Extensions.Caching.Memory;
//using Repositories.Locking;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Repositories
//{

//    public sealed class CachedRepository<T>
//        : Repository<T>
//        , ICachedRepository<T>
//        , IDisposable
//        where T : class
//    {
//        // tood: implement reader writer locker slim
//        private readonly IMemoryCache cache;
//        private readonly IRepository<T> repository;
//        private readonly MemoryCacheEntryOptions cacheEntryOptions;
//        private readonly ConcurrentHashSet<string> keys = new();
//        private bool disposedValue;

//        public event EventHandler<KeyEventArgs> CacheHit;
//        public event EventHandler<KeyEventArgs> CacheMiss;
//        public event EventHandler<CacheItemEvictedEventArgs<T>> CacheItemEvicted;

//        public CachedRepository(IRepository<T> repository, MemoryCacheEntryOptions cacheEntryOptions)
//            : base()
//        {
//            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
//            this.repository.Deleted += this.Repository_Deleted;
//            this.repository.Updated += this.Repository_Updated;

//            var cacheOptions = new MemoryCacheOptions();
//            this.cache = new MemoryCache(cacheOptions);

//            this.cacheEntryOptions = cacheEntryOptions ?? throw new ArgumentNullException(nameof(cacheEntryOptions));
//            this.cacheEntryOptions.RegisterPostEvictionCallback(this.OnPostEviction);
//        }

//        public CachedRepository(IRepository<T> repository)
//            : this(repository, new MemoryCacheEntryOptions())
//        {
//        }

//        public override int Count()
//        {
//            return this.repository.Count();
//        }

//        public override int Delete(string key)
//        {
//            if (String.IsNullOrWhiteSpace(key))
//            {
//                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
//            }

//            this.cache.Remove(key);
//            this.keys.Remove(key);
//            var deletedCount = this.repository.Delete(key);

//            this.OnDeleted(new KeyEventArgs(key));
//            return deletedCount;
//        }

//        protected override IEnumerable<Entity<T>> Entities()
//        {
//            foreach (var key in this.keys.Items())
//            {
//                if (this.TryGetEntity(key, out var entity))
//                {
//                    yield return entity;
//                }
//            }

//            foreach (var entity in (this.repository as IEntityCollection<T>).Entities(this.keys))
//            {
//                yield return entity;
//            }
//        }

//        protected override IEnumerable<Entity<T>> Entities(IEnumerable<string> excludedKeys)
//        {
//            return excludedKeys is null
//                ? throw new ArgumentNullException(nameof(excludedKeys))
//                : this.GetEntities(excludedKeys);
//        }

//        private IEnumerable<Entity<T>> GetEntities(IEnumerable<string> excludedKeys)
//        {
//            var includedKeys = this.keys.Except(excludedKeys);
//            foreach (var key in includedKeys)
//            {
//                if (this.TryGetEntity(key, out var entity))
//                {
//                    yield return entity;
//                }
//            }

//            foreach (var entity in (this.repository as IEntityCollection<T>).Entities(excludedKeys.Union(includedKeys)))
//            {
//                yield return entity;
//            }
//        }

//        public override Entity<T> Insert(T element)
//        {
//            var entity = this.repository.Insert(element);
//            _ = this.SetEntity(entity);
//            this.OnInserted(new EntityEventArgs<T>(entity));
//            return entity;
//        }

//        public override Entity<T> Select(string key)
//        {
//            if (String.IsNullOrWhiteSpace(key))
//            {
//                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
//            }

//            if (this.TryGetEntity(key, out var entity))
//            {
//                this.CacheHit?.Invoke(this, new KeyEventArgs(key));
//            }
//            else
//            {
//                entity = this.repository.Select(key);
//                this.SetEntity(entity);
//                this.CacheMiss?.Invoke(this, new KeyEventArgs(key));
//            }

//            this.OnSelected(new EntityEventArgs<T>(entity));
//            return entity;
//        }

//        public override IEnumerable<Entity<T>> Select(Func<Entity<T>, bool> predicate)
//        {
//            if (predicate is null)
//            {
//                throw new ArgumentNullException(nameof(predicate));
//            }

//            var entities = this.Entities()
//                .Where(predicate)
//                .Select(e =>
//                {
//                    this.OnSelected(new EntityEventArgs<T>(e));
//                    return e;
//                });

//            var cachedKeys = entities
//                .Where(entity => this.keys.Contains(entity.Key))
//                .Select(entity => entity.Key);

//            var uncachedKeys = entities
//                .Where(entity => !this.keys.Contains(entity.Key))
//                .Select(entity => this.SetEntity(entity))
//                .Select(entity => entity.Key);

//            this.keys.UnionWith(uncachedKeys);

//            foreach (var key in cachedKeys)
//            {
//                this.CacheHit?.Invoke(this, new KeyEventArgs(key));
//            }

//            foreach (var key in uncachedKeys)
//            {
//                this.CacheMiss?.Invoke(this, new KeyEventArgs(key));
//            }

//            return entities;
//        }

//        public override Entity<T> Update(Entity<T> entity)
//        {
//            var clone = entity is null
//                ? throw new ArgumentNullException(nameof(entity))
//                : this.repository.Update(entity);

//            this.SetEntity(clone);
//            this.OnUpdated(new EntityEventArgs<T>(clone));

//            return clone;
//        }

//        public void Dispose()
//        {
//            this.Dispose(disposing: true);
//            GC.SuppressFinalize(this);
//        }

//        private void Dispose(bool disposing)
//        {
//            if (!this.disposedValue)
//            {
//                if (disposing)
//                {
//                    this.cache.Dispose();
//                }

//                this.disposedValue = true;
//            }
//        }

//        private void Repository_Updated(object sender, EntityEventArgs<T> e)
//        {
//            if (this.keys.Contains(e.Entity.Key))
//            {
//                this.cache.Remove(e.Entity.Key);
//                this.keys.Remove(e.Entity.Key);
//                this.OnDeleted(new KeyEventArgs(e.Entity.Key));
//            }
//        }

//        private void Repository_Deleted(object sender, KeyEventArgs e)
//        {
//            if (this.keys.Contains(e.Key))
//            {
//                this.cache.Remove(e.Key);
//                this.keys.Remove(e.Key);
//                this.OnDeleted(new KeyEventArgs(e.Key));
//            }
//        }

//        private void OnPostEviction(object key, object value, EvictionReason reason, object state)
//        {
//            this.keys.Remove(key as string);
//            this.CacheItemEvicted?.Invoke(this, new CacheItemEvictedEventArgs<T>(value as Entity<T>, reason));
//        }

//        private bool TryGetEntity(string key, out Entity<T> entity)
//        {
//            return this.cache.TryGetValue(key, out entity);
//        }

//        private Entity<T> SetEntity(Entity<T> entity)
//        {
//            this.cache.Set(entity.Key, entity, this.cacheEntryOptions);
//            this.keys.Add(entity.Key);

//            return entity;
//        }
//    }
//}
