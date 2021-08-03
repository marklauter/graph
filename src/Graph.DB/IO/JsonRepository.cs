using Graph.DB.Elements;
using Graph.DB.Entities;
using Newtonsoft.Json;
using System.IO;

namespace Graph.DB.IO
{
    public sealed class JsonRepository<T>
        : FileRepository<T>
        where T : IElement
    {
        private readonly JsonSerializer jsonSerializer;

        public JsonRepository(string path)
            : base(path)
        {
            var settings = new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };

            this.jsonSerializer = JsonSerializer.Create(settings);
        }

        protected override Entity<T> StreamRead(Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(streamReader);
            return this.jsonSerializer.Deserialize<Entity<T>>(jsonReader);
        }

        protected override void StreamWrite(Entity<T> entity, Stream stream)
        {
            using var streamWriter = new StreamWriter(stream);
            using var jsonWriter = new JsonTextWriter(streamWriter);
            this.jsonSerializer.Serialize(jsonWriter, entity);
        }
    }
}
