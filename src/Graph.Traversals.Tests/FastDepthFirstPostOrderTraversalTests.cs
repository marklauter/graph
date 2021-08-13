﻿using Graph.Indexes;

namespace Graph.Traversals.Tests
{
    public sealed class FastDepthFirstPostOrderTraversalTests
        : TraversalTests
    {
        protected override ITraversal<int> CreateTraversal(IGraphIndex<int> index)
        {
            return new FastDepthFirstPostOrderTraversal(index);
        }
    }
}
