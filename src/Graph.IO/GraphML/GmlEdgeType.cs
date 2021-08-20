using System.Xml.Serialization;

namespace Graphs.IO.GraphML
{
    public enum GmlEdgeType
    {
        [XmlEnum("directed")]
        Directed,

        [XmlEnum("undirected")]
        Undirected,
    }
}
