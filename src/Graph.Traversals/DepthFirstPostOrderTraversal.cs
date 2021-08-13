using Graph.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Traversals
{
    public sealed class DepthFirstPostOrderTraversal<TKey>
        : Traversal<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public DepthFirstPostOrderTraversal(IGraphIndex<TKey> adjacencyIndex)
            : base(adjacencyIndex)
        {
        }

        public override IEnumerable<TKey> Traverse(TKey node)
        {
            return this.Traverse(node, -1);
        }

        public override IEnumerable<TKey> Traverse(TKey node, int maxDepth)
        {
            var depth = 0;
            var traversal = new Stack<TKey>();
            var visited = new HashSet<TKey>(this.AdjacencyIndex.Size);
            var neighbors = new Stack<TKey>(new TKey[] { node });

            while (neighbors.Count > 0)// && (maxDepth == -1 || depth < maxDepth))
            {
                var nextNode = neighbors.Pop();
                if (!visited.Contains(nextNode))
                {
                    ++depth;
                    traversal.Push(nextNode);
                    visited.Add(nextNode);
                    foreach (var neighbor in this.AdjacencyIndex.Neighbors(nextNode).Where(n => !visited.Contains(n)))
                    {
                        neighbors.Push(neighbor);
                    }
                }
            }

            while (traversal.Count > 0)
            {
                yield return traversal.Pop();
            }
        }
    }
}
