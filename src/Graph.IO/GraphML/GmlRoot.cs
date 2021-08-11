using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Graph.IO.GraphML
{
    [XmlRoot("graphml")]
    public sealed class GmlRoot
    {
        internal GmlRoot() : base() { }

        internal GmlRoot(IEnumerable<Elements.Graph> graphs)
        {            
            var graphAttributeKeys = graphs
                .SelectMany(g => g.Attributes.Keys)
                .Distinct()
                .Select(a => new GmlKey(GmlKeyTarget.Graph, a, "string", null))
                .ToDictionary(k => k.Name, k => k);
            graphAttributeKeys.Add("class", new GmlKey(GmlKeyTarget.Graph, "class", "string", null));

            var nodeAttributeKeys = graphs
                .SelectMany(g => g.Nodes)
                .SelectMany(n => n.Attributes.Keys)
                .Distinct()
                .Select(a => new GmlKey(GmlKeyTarget.Node, a, "string", null))
                .ToDictionary(k => k.Name, k => k);
            nodeAttributeKeys.Add("class", new GmlKey(GmlKeyTarget.Node, "class", "string", null));

            var edgeAttributeKeys = graphs
                .SelectMany(g => g.Edges)
                .SelectMany(e => e.Attributes.Keys)
                .Distinct()
                .Select(a => new GmlKey(GmlKeyTarget.Edge, a, "string", null))
                .ToDictionary(k => k.Name, k => k);
            edgeAttributeKeys.Add("class", new GmlKey(GmlKeyTarget.Edge, "class", "string", null));

            this.Keys = graphAttributeKeys.Values
                .Union(nodeAttributeKeys.Values)
                .Union(edgeAttributeKeys.Values)
                .ToList();

            this.Graphs = graphs
                .Select(g => new GmlGraph(
                    g, 
                    graphAttributeKeys,
                    nodeAttributeKeys,
                    edgeAttributeKeys))
                .ToList();
        }

        [XmlElement("graph")]
        public List<GmlGraph> Graphs { get; set; }

        [XmlElement("key")]
        public List<GmlKey> Keys { get; set; }
    }
}
