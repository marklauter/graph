﻿using Graph.Indexes;

namespace Graph.Test
{
    public class UndirectedAdjacencyListTests
        : AdjacencyIndexTests
    {
        protected override IAdjacencyIndex<int> EmptyIndex()
        {
            return UndirectedAdjacencyList.Empty;
        }
    }
}
