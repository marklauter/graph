using Graph.Graphs;
using System;
using System.Collections.Generic;

namespace Graph.Indexes
{
    public sealed class DirectedAdjacencyList<TKey>
        : AdjacencyList<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public static IAdjacencyIndex<TKey> Empty()
        {
            return new DirectedAdjacencyList<TKey>();
        }

        private DirectedAdjacencyList()
            : base()
        {
        }

        private DirectedAdjacencyList(DirectedAdjacencyList<TKey> other)
            : base(other)
        {
        }

        public override bool Adjacent(TKey source, TKey target)
        {
            return this.Index.TryGetValue(source, out var neighbors)
                && neighbors != null
                && neighbors.Contains(target);
        }

        public override object Clone()
        {
            return new DirectedAdjacencyList<TKey>(this);
        }

        public override bool Couple(TKey source, TKey target)
        {
            if (!this.Index.TryGetValue(source, out var neighbors))
            {
                neighbors = new HashSet<TKey>();
                this.Index.Add(source, neighbors);
            }

            return neighbors.Add(target);
        }

        public override bool Decouple(TKey source, TKey target)
        {
            return this.Index.TryGetValue(source, out var neighbors)
                && neighbors.Remove(target);
        }

        public override GraphType Type => GraphType.Directed;
    }
}
