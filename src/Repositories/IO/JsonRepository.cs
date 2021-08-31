using Newtonsoft.Json;
using System.IO;

namespace Repositories
{
    public sealed class JsonRepository<T>
        : FileRepository<T>
        where T : class
    {
        private readonly JsonSerializer serializer;

        public JsonRepository(string path)
            : base(path)
        {
            var settings = new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };

            this.serializer = JsonSerializer.Create(settings);
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
