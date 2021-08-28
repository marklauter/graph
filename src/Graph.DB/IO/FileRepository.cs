using Graphs.DB.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graphs.DB.IO
{
    public abstract class FileRepository<T>
        : IRepository<T>
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

        public int Delete(string key)
        {
            return this.DeleteFile(this.GetFileName(key));
        }

        public int Delete(Entity<T> entity)
        {
            return entity is null
                ? throw new ArgumentNullException(nameof(entity))
                : this.Delete(entity.Key);
        }

        public int Delete(T element)
        {
            return element is null
                 ? throw new ArgumentNullException(nameof(element))
                 : this.Delete(element.Key);
        }

        public int Delete(IEnumerable<Entity<T>> entities)
        {
            return entities is null
                ? throw new ArgumentNullException(nameof(entities))
                : entities.Sum(e => this.Delete(e));
        }

        public int Delete(IEnumerable<T> elements)
        {
            return elements is null
                ? throw new ArgumentNullException(nameof(elements))
                : elements.Sum(e => this.Delete(e));
        }

        public int Delete(Func<T, bool> predicate)
        {
            var entities = this.EnumerateEntities()
                .Select(e => e.Member)
                .Where(predicate);

            return this.Delete(entities);
        }

        public Entity<T> Insert(T element)
        {
            return element == null
                ? throw new ArgumentNullException(nameof(element))
                : this.CreateFile(this.GetFileName(element.Key), (Entity<T>)element);
        }

        public IEnumerable<Entity<T>> Insert(IEnumerable<T> elements)
        {
            return elements == null
                ? throw new ArgumentNullException(nameof(elements))
                : elements.Select(model => this.Insert(model));
        }

        public Entity<T> Read(string key)
        {
            return this.ReadFile(this.GetFileName(key));
        }

        public IEnumerable<Entity<T>> Read(IEnumerable<string> keys)
        {
            return keys.Select(key => this.Read(key));
        }

        public IEnumerable<Entity<T>> Read(Func<T, bool> predicate)
        {
            return this.EnumerateEntities()
                .Select(e => e.Member)
                .Where(predicate)
                .Select(e => e as Entity<T>);
        }

        public int Update(Entity<T> entity)
        {
            return entity == null
                ? throw new ArgumentNullException(nameof(entity))
                : this.UpdateFile(this.GetFileName(entity.Key), entity);
        }

        public int Update(T element)
        {
            return element == null
                ? throw new ArgumentNullException(nameof(element))
                : this.Update((Entity<T>)element);
        }

        public int Update(IEnumerable<Entity<T>> entities)
        {
            return entities is null
                ? throw new ArgumentNullException(nameof(entities))
                : entities.Sum(entity => this.Update(entity));
        }

        public int Update(IEnumerable<T> elements)
        {
            return elements is null
                ? throw new ArgumentNullException(nameof(elements))
                : elements.Sum(element => this.Update(element));
        }

        protected abstract Entity<T> StreamRead(Stream stream);

        protected abstract void StreamWrite(Entity<T> entity, Stream stream);

        protected virtual string GetFileExtension()
        {
            return "dat";
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

        private Entity<T> CreateFile(string fileName, Entity<T> entity)
        {
            using var stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
            this.StreamWrite(entity, stream);
            return entity;
        }

        private int UpdateFile(string fileName, Entity<T> entity)
        {
            using var stream = new FileStream(fileName, FileMode.Open, FileAccess.Write);
            this.StreamWrite(entity, stream);
            return 1;
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

        private IEnumerable<Entity<T>> EnumerateEntities()
        {
            return Directory.EnumerateFiles(this.path)
                .Select(filename => this.ReadFile(filename));
        }
    }
}
