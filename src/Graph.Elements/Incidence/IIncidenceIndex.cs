using Graphs.Elements;
using System.Collections.Generic;

namespace Graphs.Indexes
{
    public interface IIncidenceIndex
        : IEnumerable<Edge>
    {
        bool Add(Edge edge);

        IEnumerable<(Edge edge, NodeTypes nodeType)> Edges(Node node);

        IEnumerable<(Edge edge, NodeTypes nodeType)> Edges(Node node, NodeTypes type);

        bool Remove(Node node);

        bool Remove(Edge edge);
    }
}
