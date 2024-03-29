﻿using Graphs.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Graphs.IO
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

        public int Delete(Guid id)
        {
            var filename = this.GetFileName(id);
            if (File.Exists(filename))
            {
                File.Delete(filename);
                return 1;
            }

            return 0;
        }

        public int Delete(Entity<T> entity)
        {
            return entity is null
                ? throw new ArgumentNullException(nameof(entity))
                : this.Delete(entity.Id);
        }

        public int Delete(IEnumerable<Entity<T>> entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            var count = 0;
            foreach (var entity in entities)
            {
                count += this.Delete(entity);
            }

            return count;
        }

        public int Delete(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Entity<T> Insert(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var entity = (Entity<T>)model;
            using var stream = new FileStream(this.GetFileName(entity.Id), FileMode.CreateNew, FileAccess.Write);
            this.StreamWrite(entity, stream);
            return entity;
        }

        public IEnumerable<Entity<T>> Insert(IEnumerable<T> models)
        {
            return models == null
                ? throw new ArgumentNullException(nameof(models))
                : models.Select(model => this.Insert(model));
        }

        public Entity<T> Read(Guid id)
        {
            using var stream = new FileStream(this.GetFileName(id), FileMode.Open, FileAccess.Read);
            return this.StreamRead(stream);
        }

        public IEnumerable<Entity<T>> Read(IEnumerable<Guid> ids)
        {
            return ids.Select(id => this.Read(id));
        }

        public IEnumerable<Entity<T>> Read(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int Update(Entity<T> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            using var stream = new FileStream(this.GetFileName(entity.Id), FileMode.Open, FileAccess.Write);
            this.StreamWrite(entity, stream);

            return 1;
        }

        public int Update(IEnumerable<Entity<T>> entities)
        {
            return entities.Sum(entity => this.Update(entity));
        }

        protected abstract Entity<T> StreamRead(Stream stream);

        protected abstract void StreamWrite(Entity<T> entity, Stream stream);

        private string GetFileName(Guid id)
        {
            return System.IO.Path.Combine(this.path, $"{typeof(T).Name}.{id}.gdb");
        }
    }
}
