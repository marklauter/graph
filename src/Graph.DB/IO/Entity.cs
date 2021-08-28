﻿using Graphs.DB.Elements;
using Newtonsoft.Json;
using System;

namespace Graphs.DB.IO
{
    [JsonObject("entity")]
    public sealed class Entity<T>
        where T : IElement
    {
        private Entity() { }

        [JsonConstructor]
        private Entity(T member, int etag)
        {
            this.Member = member ?? throw new ArgumentNullException(nameof(member));
            this.ETag = etag;
        }

        //todo: could use attributes on member type to tag properties as part of the key. would require static type property cache. there's an example in sumo.data.
        [JsonIgnore]
        public string Key => this.Member.Key;

        [JsonProperty("member")]
        internal T Member { get; }

        public int ETag { get; private set; }

        public static explicit operator T(Entity<T> entity)
        {
            return entity.Member;
        }

        public static explicit operator Entity<T>(T member)
        {
            return new Entity<T>(member, 0);
        }
    }
}
