using Graphs.Indexes;
using System.Collections.Generic;

namespace Graphs.Traversals
{
    public sealed class FastDepthFirstPreOrderTraversal
        : Traversal<int>
    {
        public FastDepthFirstPreOrderTraversal(IAdjacencyIndex<int> adjacencyIndex)
            : base(adjacencyIndex)
        {
        }

        public override IEnumerable<int> Traverse(int node)
        {
            return this.Traverse(node, -1);
        }

        public override IEnumerable<int> Traverse(int node, int maxDepth)
        {
            var depth = 0;
            var visited = new bool[this.AdjacencyQuery.Size];
            var neighbors = new Stack<int>(new int[] { node });

            while (neighbors.Count > 0)// && (maxDepth == -1 || depth < maxDepth))
            {
                var nextNode = neighbors.Pop();
                if (!visited[nextNode])
                {
                    ++depth;
                    yield return nextNode;
                    visited[nextNode] = true;
                    for (var i = this.AdjacencyQuery.Size - 1; i >= 0; --i)
                    {
                        if (nextNode != i
                            && !visited[i]
                            && this.AdjacencyQuery.Adjacent(nextNode, i))
                        {
                            neighbors.Push(i);
                        }
                    }
                }
            }
        }
    }
}
