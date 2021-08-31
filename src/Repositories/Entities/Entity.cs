using Newtonsoft.Json;
using System;

namespace Repositories
{
    [JsonObject("entity")]
    public sealed class Entity<T>
        : ICloneable
        where T : class
    {
        private Entity()
        {
            this.ETag = Guid.NewGuid();
        }

        private Entity(Entity<T> other)
        {
            this.Member = other.Member;
            this.Key = GetKey(this.Member);
            this.ETag = Guid.NewGuid();
        }

        [JsonConstructor]
        private Entity(T member, string key, Guid etag)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            this.Member = member ?? throw new ArgumentNullException(nameof(member));
            this.Key = key;
            this.ETag = etag;
        }

        public Guid ETag { get; }

        [JsonIgnore]
        public string Key { get; }

        [JsonProperty("member")]
        internal T Member { get; }

        public object Clone()
        {
            return new Entity<T>(this);
        }

        private static string GetKey(T member)
        {
            var keys = new string[KeyInfoCache<T>.KeyProperties.Length];
            for (var i = 0; i < keys.Length; ++i)
            {
                keys[i] = KeyInfoCache<T>.KeyProperties[i].GetValue(member).ToString();
            }
            return String.Join('.', keys);
        }

        public static explicit operator T(Entity<T> entity)
        {
            return entity.Member;
        }

        public static explicit operator Entity<T>(T member)
        {
            return new Entity<T>(member, GetKey(member), Guid.NewGuid());
        }
    }
}
