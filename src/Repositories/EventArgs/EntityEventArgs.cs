using System;

namespace Repositories
{
    public sealed class EntityEventArgs<T>
        : EventArgs
        where T : class
    {
        public EntityEventArgs(Entity<T> entity)
        {
            this.Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }

        public Entity<T> Entity { get; }
    }
}


