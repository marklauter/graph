using Repositories.Locking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Repositories
{
    public abstract class FileRepository<T>
        : Repository<T>
        where T : class
    {
        protected FileRepository(string path, TimeSpan lockTimeout)
        {
            this.path = Path.Combine(!String.IsNullOrEmpty(path) ? path : "data", typeof(T).Name);

            if (!Directory.Exists(this.path))
            {
                Directory.CreateDirectory(this.path);
            }

            this.lockTimeout = lockTimeout;
        }

        private readonly string path;
        private readonly NamedLocks locks = new();
        private readonly TimeSpan lockTimeout;

        public override int Count()
        {
            return Directory.EnumerateFiles(this.path).Count();
        }

        public override int Delete(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            var filesDeleted = this.DeleteFile(this.GetFileName(key));

            this.OnDeleted(new KeyEventArgs(key));
            return filesDeleted;
        }

        protected override IEnumerable<Entity<T>> Entities()
        {
            return Directory.EnumerateFiles(this.path)
                .Select(filename => this.ReadFile(filename));
        }

        protected override IEnumerable<Entity<T>> Entities(IEnumerable<string> excludedKeys)
        {
            var excludedFileNames = excludedKeys is null
                ? throw new ArgumentNullException(nameof(excludedKeys))
                : excludedKeys.Select(key => this.GetFileName(key));

            return Directory.EnumerateFiles(this.path)
                .Where(filename => !excludedFileNames.Contains(filename))
                .Select(filename => this.ReadFile(filename));
        }

        public override Entity<T> Insert(T element)
        {
            var entity = element == null
                ? throw new ArgumentNullException(nameof(element))
                : (Entity<T>)element;

            this.CreateFile(this.GetFileName(entity.Key), entity);
            this.OnInserted(new EntityEventArgs<T>(entity));
            return entity;
        }

        public override Entity<T> Select(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            var entity = this.ReadFile(this.GetFileName(key));
            this.OnSelected(new EntityEventArgs<T>(entity));
            return entity;
        }

        public override IEnumerable<Entity<T>> Select(Func<Entity<T>, bool> predicate)
        {
            return predicate is null
                ? throw new ArgumentNullException(nameof(predicate))
                : this.Entities()
                    .Where(predicate)
                    .Select(e =>
                    {
                        this.OnSelected(new EntityEventArgs<T>(e));
                        return e;
                    });
        }

        public override Entity<T> Update(Entity<T> entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var current = this.ReadFile(this.GetFileName(entity.Key));
            if (current.ETag != entity.ETag)
            {
                throw new ETagMismatchException($"Expected: '{current.ETag}', actual: '{entity.ETag}'");
            }

            var clone = (Entity<T>)entity.Clone(); // creates new etag
            this.UpdateFile(this.GetFileName(clone.Key), clone);
            this.OnUpdated(new EntityEventArgs<T>(clone));
            return clone;
        }

        protected abstract Entity<T> StreamRead(Stream stream);

        protected abstract void StreamWrite(Entity<T> entity, Stream stream);

        protected virtual string GetFileExtension()
        {
            return "dat";
        }

        private void CreateFile(string fileName, Entity<T> entity)
        {
            this.locks.EnterWriteLock(fileName, this.lockTimeout);
            try
            {
                using var stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
                this.StreamWrite(entity, stream);
            }
            finally
            {
                this.locks.ExitWriteLock(fileName);
            }
        }

        private int DeleteFile(string fileName)
        {
            this.locks.EnterWriteLock(fileName, this.lockTimeout);
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                    return 1;
                }
            }
            finally
            {
                this.locks.ExitWriteLock(fileName);
            }

            return 0;
        }

        private string GetFileName(string key)
        {
            return Path.Combine(this.path, $"{typeof(T).Name}.{key}.{this.GetFileExtension()}");
        }

        private Entity<T> ReadFile(string fileName)
        {
            this.locks.EnterReadLock(fileName, this.lockTimeout);
            try
            {
                using var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                return this.StreamRead(stream);
            }
            finally
            {
                this.locks.ExitReadLock(fileName);
            }
        }

        private void UpdateFile(string fileName, Entity<T> entity)
        {
            this.locks.EnterWriteLock(fileName, this.lockTimeout);
            try
            {
                using var stream = new FileStream(fileName, FileMode.Open, FileAccess.Write);
                this.StreamWrite(entity, stream);
            }
            finally
            {
                this.locks.ExitWriteLock(fileName);
            }
        }
    }
}
