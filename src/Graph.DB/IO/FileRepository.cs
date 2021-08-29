using Graphs.DB.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graphs.DB.IO
{
    public abstract class FileRepository<T>
        : Repository<T>
        , IRepository<T>
        where T : IElement
    {
        protected FileRepository(string path)
        {
            this.path = Path.Combine(!String.IsNullOrEmpty(path) ? path : "data", typeof(T).Name);

            if (!Directory.Exists(this.path))
            {
                Directory.CreateDirectory(this.path);
            }
        }

        private readonly string path;

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

            var i = DeleteFile(this.GetFileName(key));
            this.OnDeleted(new KeyEventArgs(key));
            return i;
        }

        public override IEnumerable<Entity<T>> Entities()
        {
            return Directory.EnumerateFiles(this.path)
                .Select(filename => this.ReadFile(filename));
        }

        public override IEnumerable<Entity<T>> Entities(IEnumerable<string> excludedKeys)
        {
            if (excludedKeys is null)
            {
                throw new ArgumentNullException(nameof(excludedKeys));
            }

            var excludedFileNames = excludedKeys
                .Select(key => this.GetFileName(key));

            return Directory.EnumerateFiles(this.path)
                .Where(filename => !excludedFileNames.Contains(filename))
                .Select(filename => this.ReadFile(filename));
        }

        public override Entity<T> Insert(T element)
        {
            var entity = element == null
                ? throw new ArgumentNullException(nameof(element))
                : this.CreateFile(this.GetFileName(element.Key), (Entity<T>)element);
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

        public override IEnumerable<Entity<T>> Select(Func<T, bool> predicate)
        {
            return predicate is null
                ? throw new ArgumentNullException(nameof(predicate))
                : this.Entities()
                    .Select(e => e.Member)
                    .Where(predicate)
                    .Select(e =>
                    {
                        var entity = e as Entity<T>;
                        this.OnSelected(new EntityEventArgs<T>(entity));
                        return entity;
                    });
        }

        public override int Update(Entity<T> entity)
        {
            var i = entity == null
                ? throw new ArgumentNullException(nameof(entity))
                : this.UpdateFile(this.GetFileName(entity.Key), entity);
            this.OnUpdated(new EntityEventArgs<T>(entity));
            return i;
        }

        protected abstract Entity<T> StreamRead(Stream stream);

        protected abstract void StreamWrite(Entity<T> entity, Stream stream);

        protected virtual string GetFileExtension()
        {
            return "dat";
        }

        private Entity<T> CreateFile(string fileName, Entity<T> entity)
        {
            using var stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
            this.StreamWrite(entity, stream);
            return entity;
        }

        private static int DeleteFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                return 1;
            }

            return 0;
        }

        private string GetFileName(string key)
        {
            return Path.Combine(this.path, $"{typeof(T).Name}.{key}.{this.GetFileExtension()}");
        }

        private Entity<T> ReadFile(string fileName)
        {
            using var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            return this.StreamRead(stream);
        }

        private int UpdateFile(string fileName, Entity<T> entity)
        {
            using var stream = new FileStream(fileName, FileMode.Open, FileAccess.Write);
            this.StreamWrite(entity, stream);
            return 1;
        }
    }
}
