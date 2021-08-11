using Graph.Elements;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Graph.IO.GraphML
{
    [XmlRoot("edge")]
    public sealed class GmlEdge
        : GmlElement
    {
        internal GmlEdge() { }

        internal GmlEdge(Edge edge,
            Dictionary<string, GmlKey> keys)
            : base(edge, keys)
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
