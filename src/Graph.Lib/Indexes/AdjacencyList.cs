using System;
using System.Collections.Generic;

namespace Graph.Indexes
{
    public abstract class AdjacencyList<TKey>
        : AdjacencyIndex<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        protected readonly Dictionary<TKey, HashSet<TKey>> Index;

        protected AdjacencyList()
        {
            this.Index = new();
        }

        protected AdjacencyList(AdjacencyList<TKey> other)
        {
            this.Index = new Dictionary<TKey, HashSet<TKey>>(other.Index);
        }

        public override int Degree(TKey vertex)
        {
            return this.Index.TryGetValue(vertex, out var neighbors) && neighbors != null
                ? neighbors.Count
                : 0;
        }

        public override IEnumerable<TKey> Neighbors(TKey vertex)
        {
            if (this.Index.TryGetValue(vertex, out var neighbors))
            {
                foreach (var neigbhor in neighbors)
                {
                    yield return neigbhor;
                }
            }
        }

        public override int Size => this.Index.Count;
    }
}
