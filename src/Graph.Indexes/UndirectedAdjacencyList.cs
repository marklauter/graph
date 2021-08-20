using System;
using System.Collections.Generic;

namespace Graphs.Indexes
{
    public sealed class UndirectedAdjacencyList<TKey>
        : AdjacencyList<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public static IAdjacencyIndex<TKey> Empty()
        {
            return new UndirectedAdjacencyList<TKey>();
        }

        private UndirectedAdjacencyList()
            : base()
        {
        }

        private UndirectedAdjacencyList(UndirectedAdjacencyList<TKey> other)
            : base(other)
        {
        }

        public override bool Adjacent(TKey source, TKey target)
        {
            return this.Index.TryGetValue(source, out var neighbors)
                && neighbors.Contains(target);
        }

        public override object Clone()
        {
            return new UndirectedAdjacencyList<TKey>(this);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2234:Parameters should be passed in the correct order", Justification = "Recursing the call with the parameters reversed on purpose. Duh.")]
        public override bool Couple(TKey source, TKey target)
        {
            if (!this.Index.TryGetValue(source, out var neighbors))
            {
                neighbors = new HashSet<TKey>();
                this.Index.Add(source, neighbors);
            }

            if (neighbors.Add(target))
            {
                this.Couple(target, source);
                return true;
            }

            return false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2234:Parameters should be passed in the correct order", Justification = "Recursing the call with the parameters reversed on purpose. Duh.")]
        public override bool Decouple(TKey source, TKey target)
        {
            if (this.Index.TryGetValue(source, out var neighbors) && neighbors.Remove(target))
            {
                this.Decouple(target, source);
                return true;
            }

            return false;
        }

        public override IndexType Type => IndexType.Undirected;
    }
}
