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

        public DocumentCache(
            MemoryCacheEntryOptions cacheEntryOptions,
            IDocumentCollection<T> documents)
        {
            if (documents is null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            this.cache = new MemoryCache(new MemoryCacheOptions());
            this.cacheEntryOptions = cacheEntryOptions ?? throw new ArgumentNullException(nameof(cacheEntryOptions));
            this.cacheEntryOptions.RegisterPostEvictionCallback(this.OnPostEviction);

            documents.DocumentRemoved += this.Documents_DocumentRemoved;
            documents.DocumentUpdated += this.Documents_DocumentUpdated;
            documents.Cleared += this.Documents_Cleared;
        }

        private void Documents_Cleared(object sender, EventArgs e)
        {
            var c = this.cache;
            this.cache = new MemoryCache(new MemoryCacheOptions());
            c.Dispose();
        }

        private void Documents_DocumentUpdated(object sender, DocumentUpdatedEventArgs<T> e)
        {
            this.Evict(e.Document);
        }

        private void Documents_DocumentRemoved(object sender, DocumentRemovedEventArgs<T> e)
        {
            this.Evict(e.Document);
        }

        public event EventHandler<CacheReadEventArgs> CacheAccessed;
        public event EventHandler<CacheItemEvictedEventArgs<T>> CacheItemEvicted;

        public void Evict(Document<T> document)
        {
            this.cache.Remove(document.Key);
        }

        public Document<T> Read(string key, Func<string, Document<T>> read)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            if (read is null)
            {
                throw new ArgumentNullException(nameof(read));
            }

            var readType = CacheReadType.Hit;

            var document = this.cache.GetOrCreate(key, entry =>
            {
                readType = CacheReadType.Miss;
                entry.SetOptions(this.cacheEntryOptions);
                return read(key);
            });

            this.CacheAccessed?.Invoke(this, new CacheReadEventArgs(key, readType));

            return document;
        }

        public IEnumerable<Document<T>> Read(
            IEnumerable<string> keys,
            Func<string, Document<T>> read)
        {
            return keys is null
                ? throw new ArgumentNullException(nameof(keys))
                : keys.Select(key => this.Read(key, read));
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
