﻿using Graph.Indexes;
using System.Collections.Generic;

namespace Graph.Traversals
{
    public sealed class FastDepthFirstPreOrderTraversal
        : Traversal<int>
    {
        public FastDepthFirstPreOrderTraversal(IAdjacencyIndex<int> adjacencyIndex)
            : base(adjacencyIndex)
        {
        }

        public override IEnumerable<int> Traverse(int vertex)
        {
            return this.Traverse(vertex, -1);
        }

        public override IEnumerable<int> Traverse(int vertex, int depth)
        {
            var visited = new bool[this.AdjacencyIndex.Size];
            var neighbors = new Stack<int>(new int[] { vertex });

            while (neighbors.Count > 0)
            {
                var nextVertex = neighbors.Pop();
                if (!visited[nextVertex])
                {
                    yield return nextVertex;
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
        }
    }
}