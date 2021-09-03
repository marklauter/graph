using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs.Documents
{
    public sealed class DocumentCache<T>
        : IDocumentCache<T>
        , IDisposable
        where T : class
    {
        private readonly MemoryCacheEntryOptions cacheEntryOptions;
        private IMemoryCache cache;
        private bool disposedValue;

        public DocumentCache(MemoryCacheEntryOptions cacheEntryOptions)
        {
            this.cache = new MemoryCache(new MemoryCacheOptions());
            this.cacheEntryOptions = cacheEntryOptions ?? throw new ArgumentNullException(nameof(cacheEntryOptions));
            this.cacheEntryOptions.RegisterPostEvictionCallback(this.OnPostEviction);
        }

        public event EventHandler<CacheAccessedEventArgs> CacheAccessed;
        public event EventHandler<CacheItemEvictedEventArgs<T>> CacheItemEvicted;

        public void Clear()
        {
            var c = this.cache;
            this.cache = new MemoryCache(new MemoryCacheOptions());
            c.Dispose();
        }

        public void Evict(string key)
        {
            this.cache.Remove(key);
        }

        public void Evict(Document<T> document)
        {
            this.Evict(document.Key);
        }

        public Document<T> Read(string key, Func<string, Document<T>> itemFactory)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            if (itemFactory is null)
            {
                throw new ArgumentNullException(nameof(itemFactory));
            }

            var readType = CacheAccessType.Hit;

            var document = this.cache.GetOrCreate(key, entry =>
            {
                readType = CacheAccessType.Miss;
                entry.SetOptions(this.cacheEntryOptions);
                return itemFactory(key);
            });

            this.CacheAccessed?.Invoke(this, new CacheAccessedEventArgs(key, readType));

            return document;
        }

        public IEnumerable<Document<T>> Read(
            IEnumerable<string> keys,
            Func<string, Document<T>> itemFactory)
        {
            return keys is null
                ? throw new ArgumentNullException(nameof(keys))
                : keys.Select(key => this.Read(key, itemFactory));
        }

        private void OnPostEviction(object key, object value, EvictionReason reason, object state)
        {
            this.CacheItemEvicted?.Invoke(this, new CacheItemEvictedEventArgs<T>(value as Document<T>, reason));
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

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
