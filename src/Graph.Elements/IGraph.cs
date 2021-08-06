using System;
using System.Collections.Generic;

namespace Graph.Elements
{
    public interface IGraph
    {
        public bool Add(Node node);
        public int AddRange(IEnumerable<Node> nodes);
        public Edge Couple(Guid sourceId, Guid targetId);
        public Edge Couple(Node source, Node target);
        public bool Remove(Node node);
        public bool TryDecouple(Guid sourceId, Guid targetId, out Edge edge);
        public bool TryDecouple(Node source, Node target, out Edge edge);
        public bool IsDirected { get; }
    }
}
