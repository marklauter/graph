using Graph.Elements;
using System;

namespace Graph.Storage
{
    public sealed class Entity<T> where T : Element
    {
        private Entity() { }

        private Entity(T member, int etag)
        {
            this.member = member ?? throw new ArgumentNullException(nameof(member));
            this.ETag = etag;
        }

        public Guid Id => this.member.Id;

        // todo: decorate with json att so it gets stored
        private readonly T member;

        public int ETag { get; }

        public static explicit operator T(Entity<T> entity)
        {
            return entity.member;
        }

        public static explicit operator Entity<T>(T member)
        {
            return new Entity<T>(member, 0);
        }
    }
}
