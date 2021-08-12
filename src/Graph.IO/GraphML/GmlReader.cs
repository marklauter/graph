using Graph.Elements;
using System;
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
                .Select(g => g.ToGrah(root));
        }

        private static Elements.Graph ToGrah(this GmlGraph gmlGraph, GmlRoot root)
        {
            var graphAttributeKeys = root.Keys
                .Where(k => k.Target == GmlKeyTarget.Graph && String.Compare(k.Name, "class", true) != 0)
                .ToDictionary(k => k.Id);

            var graphClassKey = root.Keys
                .Single(k => k.Target == GmlKeyTarget.Graph && String.Compare(k.Name, "class", true) == 0);

            var graph = new Elements.Graph(gmlGraph.Id);

            graph.Qualify(gmlGraph, graphAttributeKeys, graphClassKey);
            graph.Classify(gmlGraph, graphClassKey);

            graph.AddNodes(root, gmlGraph);
            graph.AddEdges(root, gmlGraph);

            return graph;
        }

        private static void AddEdges(this Elements.Graph graph, GmlRoot root, GmlGraph gmlGraph)
        {
            var edgeAttributeKeys = root.Keys
                .Where(k => k.Target == GmlKeyTarget.Edge && String.Compare(k.Name, "class", true) != 0)
                .ToDictionary(k => k.Id);

            var edgeClassKey = root.Keys
                .Single(k => k.Target == GmlKeyTarget.Edge && String.Compare(k.Name, "class", true) == 0);

            foreach (var gmlEdge in gmlGraph.Edges)
            {
                var edge = new Edge(
                    gmlEdge.Id,
                    gmlEdge.Source,
                    gmlEdge.Target);

                edge.Qualify(gmlEdge, edgeAttributeKeys, edgeClassKey);
                edge.Classify(gmlEdge, edgeClassKey);
                graph.Couple(edge);
            }
        }

        private static void AddNodes(this Elements.Graph graph, GmlRoot root, GmlGraph gmlGraph)
        {
            var nodeAttributeKeys = root.Keys
                .Where(k => k.Target == GmlKeyTarget.Node && String.Compare(k.Name, "class", true) != 0)
                .ToDictionary(k => k.Id);

            var nodeClassKey = root.Keys
                .Single(k => k.Target == GmlKeyTarget.Node && String.Compare(k.Name, "class", true) == 0);

            var nodes = new List<Node>(gmlGraph.Nodes.Count);
            foreach (var gmlNode in gmlGraph.Nodes)
            {
                var node = new Node(gmlNode.Id);

                node.Qualify(gmlNode, nodeAttributeKeys, nodeClassKey);
                node.Classify(gmlNode, nodeClassKey);
                nodes.Add(node);
            }

            graph.AddRange(nodes);
        }

        private static void Qualify(this Element target, GmlElement source, Dictionary<Guid, GmlKey> attributeKeys, GmlKey classKey)
        {
            var attributes = source.Data
                .Where(d => d.Key != classKey.Id)
                .ToDictionary(d => attributeKeys[d.Key].Name, d => d.Value);
            target.Qualify(attributes);
        }

        private static void Classify(this Element target, GmlElement source, GmlKey classKey)
        {
            var classes = source.Data
                .Where(d => d.Key == classKey.Id)
                .Select(d => d.Value);
            target.Classify(classes);
        }
    }
}
