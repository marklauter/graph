using Graph.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Traversals
{
    public sealed class DepthFirstPreOrderTraversal<TKey>
        : Traversal<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public DepthFirstPreOrderTraversal(IAdjacencyIndex<TKey> adjacencyIndex)
            : base(adjacencyIndex)
        {
        }

        public override IEnumerable<TKey> Traverse(TKey vertex)
        {
            // todo: MSL - make sure I don't have to call yield return to allow for the yields in the call to traverse
            return this.Traverse(vertex, -1);
        }

        public override IEnumerable<TKey> Traverse(TKey vertex, int depth)
        {
            var visited = new HashSet<TKey>(this.AdjacencyIndex.Size);
            var neighbors = new Stack<TKey>(new TKey[] { vertex });

            while (neighbors.Count > 0)
            {
                var nextVertex = neighbors.Pop();
                if (!visited.Contains(nextVertex))
                {
                    yield return nextVertex;
                    visited.Add(nextVertex);
                    foreach (var neighbor in this.AdjacencyIndex.Neighbors(nextVertex).Where(n => !visited.Contains(n)))
                    {
                        neighbors.Push(neighbor);
                    }
                }
            }
        }
    }
}
