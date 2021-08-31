using System;
using System.Collections.Generic;

namespace Repositories
{
    public interface IRepository<T>
        where T : class
    {
        event EventHandler<KeyEventArgs> Deleted;
        event EventHandler<EntityEventArgs<T>> Inserted;
        event EventHandler<EntityEventArgs<T>> Selected;
        event EventHandler<EntityEventArgs<T>> Updated;

        int Count();

        int Delete(string key);
        int Delete(IEnumerable<string> keys);
        int Delete(Func<Entity<T>, bool> predicate);

        Entity<T> Insert(T element);
        IEnumerable<Entity<T>> Insert(IEnumerable<T> elements);

        Entity<T> Select(string key);
        IEnumerable<Entity<T>> Select(IEnumerable<string> keys);
        IEnumerable<Entity<T>> Select(Func<Entity<T>, bool> predicate);

        Entity<T> Update(Entity<T> entity);
        IEnumerable<Entity<T>> Update(IEnumerable<Entity<T>> entities);
    }
}


