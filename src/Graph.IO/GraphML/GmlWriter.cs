using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Graph.IO.GraphML
{
    public static class GmlWriter
    {
        public static void Write(IEnumerable<Elements.Graph> graphs, Stream stream)
        {
            if (graphs is null)
            {
                throw new ArgumentNullException(nameof(graphs));
            }

            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var graphML = new GmlRoot(graphs);
            var writer = new StreamWriter(stream);
            var serializer = new XmlSerializer(typeof(GmlRoot));
            serializer.Serialize(writer, graphML);
        }

        public static void Write(Elements.Graph graph, Stream stream)
        {
            if (graph is null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            Write(new[] { graph }, stream);
        }
    }
}
