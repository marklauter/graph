using Graph.Elements;
using System;
using System.Xml.Serialization;

namespace Graph.IO.GraphML
{
    [XmlRoot("edge")]
    public sealed class GmlEdge
        : GmlElement
    {
        internal GmlEdge() { }

        internal GmlEdge(Edge edge)
            : base(edge)
        {
            this.Source = edge.Source;
            this.Target = edge.Target;
        }

        [XmlAttribute("source")]
        public Guid Source { get; set; }

        [XmlAttribute("target")]
        public Guid Target { get; set; }
    }
}
