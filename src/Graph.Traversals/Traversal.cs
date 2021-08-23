using Graphs.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs.Traversals
{
    public abstract class Traversal<TKey>
        : ITraversal<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        protected readonly IAdjacencyQuery<TKey> AdjacencyQuery;

        protected Traversal(IAdjacencyQuery<TKey> adjacencyIndex)
        {
            if (adjacencyIndex is null)
            {
                throw new System.ArgumentNullException(nameof(adjacencyIndex));
            }

            this.AdjacencyQuery = adjacencyIndex;
        }

        public int Depth(TKey node)
        {
            var visited = new HashSet<TKey>(this.AdjacencyQuery.Size);
            return this.LocalDepth(new TKey[] { node }, visited, 0);
        }

        private int LocalDepth(IEnumerable<TKey> nodes, HashSet<TKey> visited, int depth)
        {
            if (!nodes.Any())
            {
                return 0;
            }

            foreach (var node in nodes)
            {
                visited.Add(node);
            }

            var localDepth = nodes.Max(n =>
                this.LocalDepth(this.AdjacencyQuery.Neighbors(n)
                    .Where(neighbor => !visited.Contains(neighbor))
                    .ToArray()
                    , visited
                    , depth));

            return Math.Max(depth, localDepth) + 1;
        }

        public abstract IEnumerable<TKey> Traverse(TKey node);

        public abstract IEnumerable<TKey> Traverse(TKey node, int maxDepth);
    }
}
