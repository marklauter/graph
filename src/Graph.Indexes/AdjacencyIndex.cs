using System;
using System.Collections.Generic;

namespace Graph.Indexes
{
    public abstract class AdjacencyIndex<TKey>
        : IAdjacencyIndex<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public abstract bool Adjacent(TKey source, TKey target);

        public abstract bool Couple(TKey source, TKey target);

        public abstract object Clone();

        public abstract bool Decouple(TKey source, TKey target);

        public abstract int Degree(TKey node);

        public abstract TKey First();

        public abstract IEnumerable<TKey> Neighbors(TKey node);

        public abstract int Size { get; }

        public abstract IndexType Type { get; }
    }
}
