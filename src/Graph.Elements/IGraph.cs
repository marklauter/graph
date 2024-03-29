﻿using System;
using System.Collections.Generic;

namespace Graphs.Elements
{
    public interface IGraph
    {
        public bool IsDirected { get; }

        public Node Add();
        public bool Add(Node node);
        public int AddRange(IEnumerable<Node> nodes);
        public Edge Couple(Guid sourceId, Guid targetId);
        public Edge Couple(Node source, Node target);
        public bool Couple(Edge edge);
        public bool Remove(Node node);
        public bool TryDecouple(Guid sourceId, Guid targetId, out Edge edge);
        public bool TryDecouple(Node source, Node target, out Edge edge);
    }
}
