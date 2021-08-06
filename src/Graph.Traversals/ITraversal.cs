using Graph.Indexes;
using System;
using System.Collections.Generic;

namespace Graph.Traversals
{
    public interface ITraversal<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public IEnumerable<TKey> Traverse(TKey vertex);
        public IEnumerable<TKey> Traverse(TKey vertex, int depth);
        public IndexType Type { get; }
    }
}
