using System;
using System.Collections.Generic;

namespace Graph.Graphs
{
    public interface IGraph<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public int Add(TKey vertex);

        public bool Adjacent(TKey vertex1, TKey vertex2);

        public void AddRange(IEnumerable<TKey> vertices);

        public IEnumerable<TKey> BreadthFirstSearch(TKey vertex);

        public IGraph<TKey> Clone();

        public void Connect(TKey vertex1, TKey vertex2);

        public int Degree(TKey vertex);

        public IEnumerable<TKey> DepthFirstSearchPostOrder(TKey vertex);

        public IEnumerable<TKey> DepthFirstSearchPreOrder(TKey vertex);

        public void Disconnect(TKey vertex1, TKey vertex2);

        public IEnumerable<TKey> Neighbors(TKey vertex);

        public int Size { get; }

        public GraphType Type { get; }
    }
}
