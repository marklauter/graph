using Graph.Elements;
using Newtonsoft.Json;
using System.IO;

namespace Graph.IO
{
    public sealed class JsonRepository<T>
        : FileRepository<T>
        where T : IElement
    {
        private readonly JsonSerializer serializer;

        public JsonRepository(string path)
            : base(path)
        {
            var settings = new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };

            this.serializer = JsonSerializer.Create(settings);
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
