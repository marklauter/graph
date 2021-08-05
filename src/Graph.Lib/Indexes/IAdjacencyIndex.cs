using Graph.Graphs;
using System;
using System.Collections.Generic;

namespace Graph.Indexes
{
    public interface IAdjacencyIndex<TKey>
        : ICloneable
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public bool Adjacent(TKey vertex1, TKey vertex2);
        public bool Couple(TKey vertex1, TKey vertex2);
        public bool Decouple(TKey vertex1, TKey vertex2);
        public int Degree(TKey vertex);
        public IEnumerable<TKey> Neighbors(TKey vertex);
        public int Size { get; }
        public GraphType Type { get; }
    }
}
