using Graph.Graphs;
using System;
using System.Collections.Generic;

namespace Graph.Indexes
{
    public sealed class UndirectedAdjacencyList<TKey>
        : AdjacencyList<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        private UndirectedAdjacencyList()
            : base()
        {
        }

        private UndirectedAdjacencyList(UndirectedAdjacencyList<TKey> other)
            : base(other)
        {
        }

        public override bool Adjacent(TKey vertex1, TKey vertex2)
        {
            return this.Index.TryGetValue(vertex1, out var neighbors)
                && neighbors.Contains(vertex2);
        }

        public override object Clone()
        {
            return new UndirectedAdjacencyList<TKey>(this);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2234:Parameters should be passed in the correct order", Justification = "Recursing the call with the parameters reversed on purpose. Duh.")]
        public override bool Couple(TKey vertex1, TKey vertex2)
        {   
            // todo: MSL - recursion is hard - make sure this doesn't always return false because of a third call to couple 
            if (!this.Index.TryGetValue(vertex1, out var neighbors))
            {
                neighbors = new HashSet<TKey>();
                this.Index.Add(vertex1, neighbors);
            }

            return neighbors.Add(vertex2) 
                && this.Couple(vertex2, vertex1);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2234:Parameters should be passed in the correct order", Justification = "Recursing the call with the parameters reversed on purpose. Duh.")]
        public override bool Decouple(TKey vertex1, TKey vertex2)
        {
            // todo: MSL - recursion is hard - make sure this doesn't always return false because of a third call to decouple 
            return this.Index.TryGetValue(vertex1, out var neighbors)
                && neighbors.Remove(vertex2)
                && this.Decouple(vertex2, vertex1);
        }

        public override GraphType Type => GraphType.Undirected;
    }
}
