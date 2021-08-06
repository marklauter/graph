using Graph.Indexes;
using System.Collections.Generic;

namespace Graph.Traversals
{
    public sealed class FastDepthFirstPostOrderTraversal
    : Traversal<int>
    {
        public FastDepthFirstPostOrderTraversal(IAdjacencyIndex<int> adjacencyIndex)
            : base(adjacencyIndex)
        {
        }

        public override IEnumerable<int> Traverse(int vertex)
        {
            // todo: MSL - make sure I don't have to call yield return to allow for the yields in the call to traverse
            return this.Traverse(vertex, -1);
        }

        public override IEnumerable<int> Traverse(int vertex, int depth)
        {
            var visitStack = new Stack<int>();
            var visited = new bool[this.AdjacencyIndex.Size];
            var neighbors = new Stack<int>(new int[] { vertex });

            while (neighbors.Count > 0)
            {
                var nextVertex = neighbors.Pop();
                if (!visited[nextVertex])
                {
                    visitStack.Push(nextVertex);
                    visited[nextVertex] = true;
                    for (var i = this.AdjacencyIndex.Size - 1; i >= 0; --i)
                    {
                        if (nextVertex != i
                            && !visited[i]
                            && this.AdjacencyIndex.Adjacent(nextVertex, i))
                        {
                            neighbors.Push(i);
                        }
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
