using System;

namespace Graph.Indexes
{
    public interface IAdjacencyIndex<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        IIncidenceIndex<TKey> ExtractIncidenceIndex();
    }
}
