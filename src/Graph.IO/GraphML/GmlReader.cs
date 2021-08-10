using Graph.Elements;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Graph.IO.GraphML
{
    public static class GmlReader
    {
        public static IEnumerable<Elements.Graph> Read(Stream stream)
        {
            var reader = new StreamReader(stream);
            var serializer = new XmlSerializer(typeof(GmlRoot));
            var root = serializer.Deserialize(reader) as GmlRoot;
            return root.Graphs
                .Select(g => GmlGraphToGrah(g));
        }

        private static Elements.Graph GmlGraphToGrah(GmlGraph gmlGraph)
        {
            var graph = new Elements.Graph(gmlGraph.Id);
            var nodes = gmlGraph.Nodes
                .Select(n => new Node(n.Id));
            graph.AddRange(nodes);

            var edges = gmlGraph.Edges
                .Select(e => new Edge(e.Id, e.Source, e.Target));
            foreach (var edge in edges)
            {
                graph.Couple(edge);
            }

            return graph;
        }
    }
}
