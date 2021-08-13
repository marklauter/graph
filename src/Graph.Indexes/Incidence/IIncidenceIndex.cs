using System;
using System.Collections.Generic;

namespace Graph.Indexes
{
    public interface IIncidenceIndex<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        IEnumerable<(TKey key, NodeType type)> Edges(TKey node);
    }
}
