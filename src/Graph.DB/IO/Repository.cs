using Graphs.DB.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs.DB.IO
{
    public abstract class Repository<T>
        : IRepository<T>
        where T : IElement
    {
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
            var elements = (this as IRepository<T>).Entities()
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

        public abstract Entity<T> Read(string key);

        public IEnumerable<Entity<T>> Read(IEnumerable<string> keys)
        {
            return keys.Select(key => this.Read(key));
        }

        public abstract IEnumerable<Entity<T>> Read(Func<T, bool> predicate);
        
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
    }
}