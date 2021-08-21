using System;
using System.Collections.Generic;

namespace Graphs.Indexes
{
    public interface IAdjacencyQuery<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        bool Adjacent(TKey source, TKey target);

        int Degree(TKey node);

        IEnumerable<TKey> Neighbors(TKey node);

        int Size { get; }
    }
}
