using System;

namespace graph.lib.storage
{
    public sealed class Entity<T> where T : Element
    {
        private Entity() { }

        public Entity(Guid id, T member)
        {
            this.Id = id;
            this.Member = member;
        }

        public Guid Id { get; }

        public T Member { get; }
    }
}
