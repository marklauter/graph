using Graph.Elements;
using System.Xml.Serialization;

namespace Graph.IO.GraphML
{
    [XmlRoot("node")]
    public sealed class GmlNode
        : GmlElement
    {
        internal GmlNode() : base() { }

        internal GmlNode(Node node)
            : base(node)
        {
        }
    }
}
