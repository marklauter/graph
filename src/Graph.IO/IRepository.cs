using Graphs.Elements;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Graphs.IO
{
    public interface IRepository<T>
        where T : IElement
    {
        int Delete(Guid id);
        int Delete(Entity<T> entity);
        int Delete(IEnumerable<Entity<T>> entities);
        int Delete(Expression<Func<T, bool>> predicate);

        Entity<T> Insert(T model);
        IEnumerable<Entity<T>> Insert(IEnumerable<T> models);

        int Update(Entity<T> entity);
        int Update(IEnumerable<Entity<T>> entities);

        Entity<T> Read(Guid id);
        IEnumerable<Entity<T>> Read(IEnumerable<Guid> ids);
        IEnumerable<Entity<T>> Read(Expression<Func<T, bool>> predicate);
    }
}


