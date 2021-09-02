using Newtonsoft.Json;
using System;
using System.IO;

namespace Repositories
{
    public sealed class JsonRepository<T>
        : FileRepository<T>
        where T : class
    {
        private readonly JsonSerializer serializer;

        public JsonRepository(string path, TimeSpan lockTimeout)
            : base(path, lockTimeout)
        {
        }

        protected override string GetFileExtension()
        {
            return "json";
        }

        protected override Entity<T> StreamRead(Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(streamReader);
            return this.serializer.Deserialize<Entity<T>>(jsonReader);
        }

        protected override void StreamWrite(Entity<T> entity, Stream stream)
        {
            using var streamWriter = new StreamWriter(stream);
            using var jsonWriter = new JsonTextWriter(streamWriter);
            this.serializer.Serialize(jsonWriter, entity);
        }
    }
}
