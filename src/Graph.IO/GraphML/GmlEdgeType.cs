using System.Xml.Serialization;

namespace Graph.IO.GraphML
{
    public enum GmlEdgeType
    {
        [XmlEnum("directed")]
        Directed,

        [XmlEnum("undirected")]
        Undirected,
    }
}
