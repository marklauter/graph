using System;
using System.Collections.Generic;

namespace Graph.Graphs
{
    public interface IGraph<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public int Add(TKey vertex);

        public bool Adjacent(TKey source, TKey target);

        public void AddRange(IEnumerable<TKey> vertices);

        public IEnumerable<TKey> BreadthFirstSearch(TKey vertex);

        public IGraph<TKey> Clone();

        public void Connect(TKey source, TKey target);

        public int Degree(TKey vertex);

        public IEnumerable<TKey> DepthFirstSearchPostOrder(TKey vertex);

        public IEnumerable<TKey> DepthFirstSearchPreOrder(TKey vertex);

        public void Disconnect(TKey source, TKey target);

        public IEnumerable<TKey> Neighbors(TKey vertex);

        public int Size { get; }

        public GraphType Type { get; }
    }
}
