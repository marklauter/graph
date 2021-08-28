using Graphs.DB.Elements;
using System;
using System.Collections.Generic;

namespace Graphs.DB.IO
{
    public interface IRepository<T>
        where T : IElement
    {
        event EventHandler<KeyEventArgs> Deleted;
        event EventHandler<EntityEventArgs<T>> Inserted;
        event EventHandler<EntityEventArgs<T>> Selected;
        event EventHandler<EntityEventArgs<T>> Updated;

        int Count();

        int Delete(string key);
        int Delete(Entity<T> entity);
        int Delete(T element);
        int Delete(IEnumerable<Entity<T>> entities);
        int Delete(IEnumerable<T> elements);
        int Delete(Func<T, bool> predicate);

        IEnumerable<Entity<T>> Entities();
        IEnumerable<Entity<T>> Entities(IEnumerable<string> excludedKeys);

        Entity<T> Insert(T element);
        IEnumerable<Entity<T>> Insert(IEnumerable<T> elements);

        int Update(Entity<T> entity);
        int Update(T element);
        int Update(IEnumerable<Entity<T>> entities);
        int Update(IEnumerable<T> elements);

        Entity<T> Select(string key);
        IEnumerable<Entity<T>> Select(IEnumerable<string> keys);
        IEnumerable<Entity<T>> Select(Func<T, bool> predicate);
    }
}


