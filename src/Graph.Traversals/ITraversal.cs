using System;
using System.Collections.Generic;

namespace Graphs.Traversals
{
    // todo: traversals can be refactored into a set of extension methods
    public interface ITraversal<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        //todo: move the forest concept to Graph - traversal doesn't have enough information to accomplish this as traversal implementations are dependent upon adjacency indexes which are edge centric and don't necessarily contain the full set of nodes.
        // public IEnumerable<IEnumerable<TKey>> Forests();

        public int Depth(TKey node);
        public IEnumerable<TKey> Traverse(TKey node);
        public IEnumerable<TKey> Traverse(TKey node, int maxDepth);
    }
}
