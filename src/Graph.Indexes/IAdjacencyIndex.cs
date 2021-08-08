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
        public int Degree(TKey node);
        
        /// <summary>
        /// Returns the first node in the index. Depending on the graph structure, the selection of the first node may be arbitrary or critically important.
        /// For example, in a binary tree, the node selected to represent the first item in the tree must be the root node.
        /// </summary>
        /// <returns>TKey</returns>
        public TKey First();
        public IEnumerable<TKey> Neighbors(TKey node);
        public int Size { get; }
        public IndexType Type { get; }
    }
}
