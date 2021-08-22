using System;
using System.Collections.Generic;

namespace Graphs.Indexes
{
    public interface IAdjacencyIndex<TKey>
        : IAdjacencyQuery<TKey>
        , ICloneable
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        
        bool Couple(TKey source, TKey target);
        
        bool Decouple(TKey source, TKey target);
        
        /// <summary>
        /// Returns the first node in the index. Depending on the graph structure, the selection of the first node may be arbitrary or critically important.
        /// For example, in a binary tree, the node selected to represent the first item in the tree must be the root node.
        /// </summary>
        /// <returns>TKey</returns>
        TKey First();

        IEnumerable<TKey> Keys();
        
        void Parse(string matrix);

        IndexType Type { get; }
    }
}
