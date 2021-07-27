using graph.elements;
using System;

namespace graph.storage
{
    public sealed class Entity<T> where T : Element
    {
        private Entity() { }

        public Entity(T member)
        {
            this.Member = member;
        }

        public Guid Id => this.Member.Id;

        public T Member { get; }

        public int ETag { get; }
    }
}
