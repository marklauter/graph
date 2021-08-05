using Graph.Graphs;
using System;
using System.Collections.Generic;

namespace Graph.Indexes
{
    //public TKey[] DepthFirstSearchPostOrder(TKey vertex)
    //{
    //    var vertices = new int[this.Size];
    //    var visitStack = new Stack<int>();
    //    var visited = new bool[this.Size];
    //    var neighbors = new Stack<int>(new int[] { vertex });

    //    while (neighbors.Count > 0)
    //    {
    //        var v = neighbors.Pop();
    //        if (!visited[v])
    //        {
    //            visitStack.Push(v);
    //            visited[v] = true;
    //            for (var i = this.Size - 1; i >= 0; --i)
    //            {
    //                if (i != v && this.Adjacent(v, i) && !visited[i])
    //                {
    //                    neighbors.Push(i);
    //                }
    //            }
    //        }
    //    }

    //    for (var i = 0; i < vertices.Length; ++i)
    //    {
    //        vertices[i] = visitStack.Pop();
    //    }

    //    return vertices;
    //}

    //public TKey[] DepthFirstSearchPreOrder(TKey vertex)
    //{
    //    var vertices = new int[this.Size];
    //    var visited = new bool[this.Size];
    //    var neighbors = new Stack<int>(new int[] { vertex });
    //    var vi = 0;

    //    while (neighbors.Count > 0)
    //    {
    //        var v = neighbors.Pop();
    //        if (!visited[v])
    //        {
    //            vertices[vi] = v;
    //            ++vi;
    //            visited[v] = true;
    //            for (var i = this.Size - 1; i >= 0; --i)
    //            {
    //                if (i != v && this.Adjacent(v, i) && !visited[i])
    //                {
    //                    neighbors.Push(i);
    //                }
    //            }
    //        }
    //    }

    //    return vertices;
    //}

    //public TKey[] BreadthFirstSearch(TKey vertex)
    //{
    //    throw new System.NotImplementedException();
    //}

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
            var visited = new HashSet<TKey>(this.AdjacencyIndex.Size);
            var neighbors = new Stack<TKey>(new TKey[] { vertex });

            while (neighbors.Count > 0)
            {
                var v = neighbors.Pop();
                if (!visited.Contains(v))
                {
                    yield return v;
                    visited.Add(v);
                    foreach(var neighbor in this.AdjacencyIndex.Neighbors(v))
                    {
                        if (!visited.Contains(neighbor))
                        {
                            neighbors.Push(neighbor);
                        }
                    }
                }
            }
        }

        public override IEnumerable<TKey> Traverse(TKey vertex, int depth)
        {
            throw new NotImplementedException();
        }
    }

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

        public abstract IEnumerable<TKey> Traverse(TKey vertex);
        
        public abstract IEnumerable<TKey> Traverse(TKey vertex, int depth);

        public GraphType Type => this.AdjacencyIndex.Type;
    }
}
