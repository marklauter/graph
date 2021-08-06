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
        public DepthFirstPostOrderTraversal(IAdjacencyIndex<TKey> adjacencyIndex)
            : base(adjacencyIndex)
        {
        }

        public override IEnumerable<TKey> Traverse(TKey vertex)
        {
            return this.Traverse(vertex, -1);
        }

        public override IEnumerable<TKey> Traverse(TKey vertex, int depth)
        {
            var visitStack = new Stack<TKey>();
            var visited = new HashSet<TKey>(this.AdjacencyIndex.Size);
            var neighbors = new Stack<TKey>(new TKey[] { vertex });

            while (neighbors.Count > 0)
            {
                var nextVertex = neighbors.Pop();
                if (!visited.Contains(nextVertex))
                {
                    visitStack.Push(nextVertex);
                    visited.Add(nextVertex);
                    foreach (var neighbor in this.AdjacencyIndex.Neighbors(nextVertex).Where(n => !visited.Contains(n)))
                    {
                        neighbors.Push(neighbor);
                    }
                }
            }

            while (visitStack.Count > 0)
            {
                yield return visitStack.Pop();
            }
        }
    }
}
