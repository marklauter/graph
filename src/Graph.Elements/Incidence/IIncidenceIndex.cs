﻿using Graphs.Elements;
using System;
using System.Collections.Generic;

namespace Graphs.Indexes
{
    public interface IIncidenceIndex
        : IEnumerable<Edge>
        , ICloneable
    {
        bool Add(Edge edge);

        IEnumerable<(Edge edge, NodeTypes nodeType)> Edges(Node node);

        IEnumerable<(Edge edge, NodeTypes nodeType)> Edges(Node node, NodeTypes type);

        bool Remove(Node node);

        bool Remove(Edge edge);
    }
}