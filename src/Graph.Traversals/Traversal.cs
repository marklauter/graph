using Graph.Indexes;
using System;
using System.Collections.Generic;

namespace Graph.Traversals
{
    public abstract class Traversal<TKey>
        : ITraversal<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        protected readonly IAdjacencyIndex<TKey> AdjacencyIndex;

        protected Traversal(IAdjacencyIndex<TKey> adjacencyIndex)
        {
            if (adjacencyIndex is null)
            {
                throw new System.ArgumentNullException(nameof(adjacencyIndex));
            }

            this.AdjacencyIndex = adjacencyIndex;
        }

        public abstract int Depth(TKey node);

        public abstract IEnumerable<TKey> Traverse(TKey node);

        public abstract IEnumerable<TKey> Traverse(TKey node, int maxDepth);

        public IndexType Type => this.AdjacencyIndex.Type;
    }
}
