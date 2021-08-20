using Graphs.Elements;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Graphs.IO.GraphML
{
    [XmlRoot("node")]
    public sealed class GmlNode
        : GmlElement
    {
        internal GmlNode() : base() { }

        internal GmlNode(
            Node node,
            Dictionary<string, GmlKey> keys)
            : base(node, keys)
        {
        }
    }
}
