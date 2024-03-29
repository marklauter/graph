﻿using Graphs.Indexes;
using System.Collections.Generic;

namespace Graphs.Traversals
{
    public sealed class FastDepthFirstPostOrderTraversal
        : Traversal<int>
    {
        public FastDepthFirstPostOrderTraversal(IAdjacencyIndex<int> adjacencyIndex)
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
            var traversal = new Stack<int>();
            var visited = new bool[this.AdjacencyIndex.Size];
            var neighbors = new Stack<int>(new int[] { node });

            while (neighbors.Count > 0)// && (maxDepth == -1 || depth < maxDepth))
            {
                var nextNode = neighbors.Pop();
                if (!visited[nextNode])
                {
                    ++depth;
                    traversal.Push(nextNode);
                    visited[nextNode] = true;
                    for (var i = this.AdjacencyIndex.Size - 1; i >= 0; --i)
                    {
                        if (nextNode != i
                            && !visited[i]
                            && this.AdjacencyIndex.Adjacent(nextNode, i))
                        {
                            neighbors.Push(i);
                        }
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
