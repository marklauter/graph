using Newtonsoft.Json;
using System.IO;

namespace Graphs.Documents.IO
{
    public sealed class JsonDocumentSerializer<T>
        : IDocumentSerializer<T>
        where T : class
    {
        private readonly JsonSerializer serializer;

        public JsonDocumentSerializer()
            : this(Formatting.Indented)
        {

        }

        public JsonDocumentSerializer(Formatting formatting)
        {
            var settings = new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = formatting,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };

            this.serializer = JsonSerializer.Create(settings);
        }

        public Document<T> Deserialize(Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(streamReader);
            return this.serializer.Deserialize<Document<T>>(jsonReader);
        }

        public void Serialize(Document<T> document, Stream stream)
        {
            using var streamWriter = new StreamWriter(stream);
            using var jsonWriter = new JsonTextWriter(streamWriter);
            this.serializer.Serialize(jsonWriter, document);
        }
    }
}
