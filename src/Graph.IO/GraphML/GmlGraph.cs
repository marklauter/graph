using Graphs.Elements;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Graphs.IO.GraphML
{
    [XmlRoot("graph")]
    public sealed class GmlGraph
        : GmlElement
    {
        internal GmlGraph() : base() { }

        internal GmlGraph(
            Graph graph,
            Dictionary<string, GmlKey> graphKeys,
            Dictionary<string, GmlKey> nodeKeys,
            Dictionary<string, GmlKey> edgeKeys)
            : base(graph, graphKeys)
        {
            this.Nodes = graph.Nodes
                .Select(n => new GmlNode(n, nodeKeys))
                .ToList();

            this.Edges = graph.Edges
                .Select(e => new GmlEdge(e, edgeKeys))
                .ToList();

            this.EdgeDefault = graph.IsDirected
                ? GmlEdgeType.Directed
                : GmlEdgeType.Undirected;
        }

        [XmlElement("node")]
        public List<GmlNode> Nodes { get; set; }

        [XmlElement("edge")]
        public List<GmlEdge> Edges { get; set; }

        [XmlAttribute("edgedefault")]
        public GmlEdgeType EdgeDefault { get; set; }
    }
}
