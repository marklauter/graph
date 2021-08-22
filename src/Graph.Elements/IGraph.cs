using Graphs.Indexes;
using System;
using System.Collections.Generic;

namespace Graphs.Elements
{
    public interface IGraph
        : IAdjacencyQuery<Guid>
    {
        bool IsDirected { get; }
        IEnumerable<Node> Nodes { get; }
        IEnumerable<Edge> Edges { get; }

        Node Add();
        bool Add(Node node);
        int AddRange(IEnumerable<Node> nodes);
        Edge Couple(Guid sourceId, Guid targetId);
        Edge Couple(Node source, Node target);
        bool Couple(Edge edge);
        IEnumerable<(Edge edge, NodeTypes nodeType)> IncidentEdges(Node node);
        IEnumerable<Node> Neighbors(Node node);
        bool Remove(Node node);
        bool TryDecouple(Guid sourceId, Guid targetId, out Edge edge);
        bool TryDecouple(Node source, Node target, out Edge edge);
    }
}
