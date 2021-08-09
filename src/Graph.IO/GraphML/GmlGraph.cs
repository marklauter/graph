using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Graph.IO.GraphML
{
    [XmlRoot("graph")]
    public sealed class GmlGraph
        : GmlElement
    {
        internal GmlGraph() : base() { }

        internal GmlGraph(Elements.Graph graph)
            : base(graph)
        {
            this.Nodes = graph.Nodes
                .Select(n => new GmlNode(n))
                .ToList();

            this.Edges = graph.Edges
                .Select(e => new GmlEdge(e))
                .ToList();
        }

        [XmlElement("node")]
        public List<GmlNode> Nodes { get; set; }

        [XmlElement("edge")]
        public List<GmlEdge> Edges { get; set; }
    }
}
