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
                .Select(g => GmlGraphToGrah(root, g));
        }

        private static Elements.Graph GmlGraphToGrah(GmlRoot root, GmlGraph gmlGraph)
        {
            var graphAttributeKeys = root.Keys
                .Where(k => k.Target == GmlKeyTarget.Graph && String.Compare(k.Name, "class", true) != 0 )
                .ToDictionary(k => k.Id);

            var graphClassKey = root.Keys
                .Single(k => k.Target == GmlKeyTarget.Graph && String.Compare(k.Name, "class", true) == 0);

            var graph = new Elements.Graph(gmlGraph.Id);
            
            QualifyElement(gmlGraph, graph, graphAttributeKeys, graphClassKey);
            ClassifyElement(gmlGraph, graph, graphClassKey);
            
            AddNodes(root, gmlGraph, graph);
            AddEdges(root, gmlGraph, graph);

            return graph;
        }

        private static void AddEdges(GmlRoot root, GmlGraph gmlGraph, Elements.Graph graph)
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

                QualifyElement(gmlEdge, edge, edgeAttributeKeys, edgeClassKey);
                ClassifyElement(gmlEdge, edge, edgeClassKey);
                graph.Couple(edge);
            }
        }

        private static void AddNodes(GmlRoot root, GmlGraph gmlGraph, Elements.Graph graph)
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

                QualifyElement(gmlNode, node, nodeAttributeKeys, nodeClassKey);
                ClassifyElement(gmlNode, node, nodeClassKey);
                nodes.Add(node);
            }

            graph.AddRange(nodes);
        }

        private static void QualifyElement(GmlElement source, Element target, Dictionary<Guid, GmlKey> attributeKeys, GmlKey classKey)
        {
            var attributes = source.Data
                .Where(d=> d.Key != classKey.Id)
                .ToDictionary(d => attributeKeys[d.Key].Name, d => d.Value);
            target.Qualify(attributes);
        }

        private static void ClassifyElement(GmlElement source, Element target, GmlKey classKey)
        {
            var classes = source.Data
                .Where(d => d.Key == classKey.Id)
                .Select(d => d.Value);
            target.Classify(classes);
        }
    }
}
