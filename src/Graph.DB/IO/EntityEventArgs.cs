using Graphs.DB.Elements;
using System;

namespace Graphs.DB.IO
{
    public sealed class EntityEventArgs<T>
        : EventArgs
        where T : IElement
    {
        public EntityEventArgs(Entity<T> entity)
        {
            this.Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }

        public Entity<T> Entity { get; }
    }
}


