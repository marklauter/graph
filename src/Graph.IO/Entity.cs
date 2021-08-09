using Graph.Elements;
using Newtonsoft.Json;
using System;

namespace Graph.IO
{
    public sealed class Entity<T>
        where T : IElement
    {
        private Entity() { }

        private Entity(T member, int etag)
        {
            this.member = member ?? throw new ArgumentNullException(nameof(member));
            this.ETag = etag;
        }

        [JsonIgnore]
        public Guid Id => this.member.Id;

        [JsonProperty]
        private readonly T member;

        public int ETag { get; private set; }

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


