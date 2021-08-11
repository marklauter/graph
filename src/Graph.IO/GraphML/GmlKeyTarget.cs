using System.Xml.Serialization;

namespace Graph.IO.GraphML
{
    public enum GmlKeyTarget
    {
        [XmlEnum("undefined")]
        Undefined,

        [XmlEnum("edge")]
        Edge,

        [XmlEnum("graph")]
        Graph,

        [XmlEnum("node")]
        Node,
    }
}
