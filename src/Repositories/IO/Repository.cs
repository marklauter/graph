using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
    public abstract class Repository<T>
        : IDocumentCollection<T>
        , IEntityCollection<T>
        where T : class
    {
        public event EventHandler<KeyEventArgs> Deleted;
        public event EventHandler<EntityEventArgs<T>> Inserted;
        public event EventHandler<EntityEventArgs<T>> Selected;
        public event EventHandler<EntityEventArgs<T>> Updated;

        public abstract int Count();

        public abstract int Delete(string key);

        public int Delete(IEnumerable<string> keys)
        {
            return keys == null
                ? throw new ArgumentNullException(nameof(keys))
                : keys.Sum(k => this.Delete(k));
        }

        public int Delete(Func<Entity<T>, bool> predicate)
        {
            var elements = this.Entities()
                .Where(predicate);

            return this.Delete(elements.Select(e => e.Key));
        }

        protected abstract IEnumerable<Entity<T>> Entities();

        protected abstract IEnumerable<Entity<T>> Entities(IEnumerable<string> excludedKeys);

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

        public abstract IEnumerable<Entity<T>> Select(Func<Entity<T>, bool> predicate);

        public abstract Entity<T> Update(Entity<T> entity);

        public IEnumerable<Entity<T>> Update(IEnumerable<Entity<T>> entities)
        {
            return entities is null
                ? throw new ArgumentNullException(nameof(entities))
                : entities.Select(entity => this.Update(entity));
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

        IEnumerable<Entity<T>> IEntityCollection<T>.Entities()
        {
            return this.Entities();
        }

        IEnumerable<Entity<T>> IEntityCollection<T>.Entities(IEnumerable<string> excludedKeys)
        {
            return this.Entities(excludedKeys);
        }
    }
}