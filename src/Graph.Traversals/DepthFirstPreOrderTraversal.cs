﻿using Graphs.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs.Traversals
{
    public sealed class DepthFirstPreOrderTraversal<TKey>
        : Traversal<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public DepthFirstPreOrderTraversal(IAdjacencyIndex<TKey> adjacencyIndex)
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
            var visited = new HashSet<TKey>(this.AdjacencyIndex.Size);
            var neighbors = new Stack<TKey>(new TKey[] { node });

            while (neighbors.Count > 0)// && (maxDepth == -1 || depth < maxDepth))
            {
                var nextNode = neighbors.Pop();
                if (!visited.Contains(nextNode))
                {
                    ++depth;
                    yield return nextNode;
                    visited.Add(nextNode);
                    foreach (var neighbor in this.AdjacencyIndex.Neighbors(nextNode).Where(n => !visited.Contains(n)))
                    {
                        neighbors.Push(neighbor);
                    }
                }
            }
        }
    }
}
