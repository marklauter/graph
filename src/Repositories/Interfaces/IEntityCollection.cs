using System.Collections.Generic;

namespace Repositories
{
    internal interface IEntityCollection<T>
        where T : class
    {
        IEnumerable<Entity<T>> Entities();
        IEnumerable<Entity<T>> Entities(IEnumerable<string> excludedKeys);
    }
}


