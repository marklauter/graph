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
            // todo: work out how to extract the data keys from the edges and nodes
            this.Graphs = graphs
                .Select(g => new GmlGraph(g))
                .ToList();
        }

        [XmlElement("graph")]
        public List<GmlGraph> Graphs { get; set; }
    }
}
