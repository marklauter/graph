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

        public override bool Adjacent(TKey vertex1, TKey vertex2)
        {
            return this.Index.TryGetValue(vertex1, out var neighbors)
                && neighbors != null
                && neighbors.Contains(vertex2);
        }

        public override object Clone()
        {
            return new DirectedAdjacencyList<TKey>(this);
        }

        public override bool Couple(TKey vertex1, TKey vertex2)
        {
            if (!this.Index.TryGetValue(vertex1, out var neighbors))
            {
                neighbors = new HashSet<TKey>();
                this.Index.Add(vertex1, neighbors);
            }

            return neighbors.Add(vertex2);
        }

        public override bool Decouple(TKey vertex1, TKey vertex2)
        {
            return this.Index.TryGetValue(vertex1, out var neighbors)
                && neighbors.Remove(vertex2);
        }

        public override GraphType Type => GraphType.Directed;
    }
}
