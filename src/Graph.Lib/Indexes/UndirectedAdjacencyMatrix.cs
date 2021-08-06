using Graph.Graphs;
using System;

namespace Graph.Indexes
{
    public sealed class UndirectedAdjacencyMatrix
        : AdjacencyMatrix
    {
        public static IAdjacencyIndex<int> Empty()
        {
            return new UndirectedAdjacencyMatrix();
        }

        private UndirectedAdjacencyMatrix()
            : base()
        {
        }

        private UndirectedAdjacencyMatrix(AdjacencyMatrix other)
            : base(other)
        {
        }

        public override bool Adjacent(int source, int target)
        {
            return source < this.Size
                && target < this.Size
                && this.Matrix[source, target]
                && this.Matrix[target, source];
        }

        public override object Clone()
        {
            return new UndirectedAdjacencyMatrix(this);
        }

        public override bool Couple(int source, int target)
        {
            if (source >= this.Size || target >= this.Size)
            {
                this.Grow(Math.Max(source, target));
            }

            if (!this.Matrix[source, target])
            {
                this.Matrix[source, target] = true;
                this.Matrix[target, source] = true;
                return true;
            }

            return false;
        }

        public override bool Decouple(int source, int target)
        {
            if (this.Adjacent(source, target))
            {
                this.Matrix[source, target] = false;
                this.Matrix[target, source] = false;
                return true;
            }

            return false;
        }

        public override GraphType Type => GraphType.Undirected;
    }
}
