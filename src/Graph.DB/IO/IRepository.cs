using Graphs.DB.Elements;
using System;
using System.Collections.Generic;

namespace Graphs.DB.IO
{
    public interface IRepository<T>
        where T : IElement
    {
        int Delete(string key);
        int Delete(Entity<T> entity);
        int Delete(T element);
        int Delete(IEnumerable<Entity<T>> entities);
        int Delete(IEnumerable<T> elements);
        int Delete(Func<T, bool> predicate);

        Entity<T> Insert(T element);
        IEnumerable<Entity<T>> Insert(IEnumerable<T> elements);

        int Update(Entity<T> entity);
        int Update(T element);
        int Update(IEnumerable<Entity<T>> entities);
        int Update(IEnumerable<T> elements);

        Entity<T> Read(string key);
        IEnumerable<Entity<T>> Read(IEnumerable<string> keys);
        IEnumerable<Entity<T>> Read(Func<T, bool> predicate);
    }
}


