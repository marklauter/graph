using System;
using System.Collections.Generic;

namespace Graph.Indexes
{
    public interface IAdjacencyIndex<TKey>
        : ICloneable
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public bool Adjacent(TKey source, TKey target);
        public bool Couple(TKey source, TKey target);
        public bool Decouple(TKey source, TKey target);
        public int Degree(TKey vertex);
        public IEnumerable<TKey> Neighbors(TKey vertex);
        public int Size { get; }
        public IndexType Type { get; }
    }
}
