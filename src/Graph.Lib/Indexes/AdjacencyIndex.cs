using Graph.Graphs;
using System;
using System.Collections.Generic;

namespace Graph.Indexes
{
    public abstract class AdjacencyIndex<TKey>
        : IAdjacencyIndex<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public abstract bool Adjacent(TKey vertex1, TKey vertex2);

        public abstract bool Couple(TKey vertex1, TKey vertex2);

        public abstract object Clone();

        public abstract bool Decouple(TKey vertex1, TKey vertex2);

        public abstract int Degree(TKey vertex);

        public abstract IEnumerable<TKey> Neighbors(TKey vertex);

        public abstract int Size { get; }

        public abstract GraphType Type { get; }
    }
}
