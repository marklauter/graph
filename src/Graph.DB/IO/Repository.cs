﻿using Graphs.DB.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs.DB.IO
{
    public abstract class Repository<T>
        : IRepository<T>
        where T : IElement
    {
        public event EventHandler<KeyEventArgs> Deleted;
        public event EventHandler<EntityEventArgs<T>> Inserted;
        public event EventHandler<EntityEventArgs<T>> Selected;
        public event EventHandler<EntityEventArgs<T>> Updated;

        public abstract int Count();
        
        public abstract int Delete(string key);
        
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
            var elements = this.Entities()
                .Select(e => e.Member)
                .Where(predicate);

            return this.Delete(elements);
        }

        public abstract IEnumerable<Entity<T>> Entities();

        public abstract IEnumerable<Entity<T>> Entities(IEnumerable<string> excludedKeys);

        public abstract Entity<T> Insert(T element);

        public IEnumerable<Entity<T>> Insert(IEnumerable<T> elements)
        {
            return elements == null
                ? throw new ArgumentNullException(nameof(elements))
                : elements.Select(model => this.Insert(model));
        }

        public abstract Entity<T> Select(string key);

        public IEnumerable<Entity<T>> Select(IEnumerable<string> keys)
        {
            return keys.Select(key => this.Select(key));
        }

        public abstract IEnumerable<Entity<T>> Select(Func<T, bool> predicate);
        
        public abstract int Update(Entity<T> entity);

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

        protected void OnDeleted(KeyEventArgs args)
        {
            this.Deleted?.Invoke(this, args);
        }

        protected void OnInserted(EntityEventArgs<T> args)
        {
            this.Inserted?.Invoke(this, args);
        }

        protected void OnSelected(EntityEventArgs<T> args)
        {
            this.Selected?.Invoke(this, args);
        }

        protected void OnUpdated(EntityEventArgs<T> args)
        {
            this.Updated?.Invoke(this, args);
        }
    }
}