using System;

namespace Graphs.Indexes
{
    public sealed class DirectedAdjacencyMatrix
        : AdjacencyMatrix
    {
        public static IAdjacencyIndex<int> Empty()
        {
            return new DirectedAdjacencyMatrix();
        }

        private DirectedAdjacencyMatrix()
            : base()
        {
        }

        private DirectedAdjacencyMatrix(AdjacencyMatrix other)
            : base(other)
        {
        }

        public override bool Adjacent(int source, int target)
        {
            return source < this.Size
                && target < this.Size
                && this.Matrix[source, target];
        }

        public override object Clone()
        {
            return new DirectedAdjacencyMatrix(this);
        }

        public override bool Couple(int source, int target)
        {
            if (!this.Adjacent(source, target))
            {
                if (source >= this.Size || target >= this.Size)
                {
                    this.Grow(Math.Max(source, target));
                }

                this.Matrix[source, target] = true;
                return true;
            }

            return false;
        }

        public override bool Decouple(int source, int target)
        {
            if (this.Adjacent(source, target))
            {
                this.Matrix[source, target] = false;
                return true;
            }

            return false;
        }

        public override IndexType Type => IndexType.Directed;
    }
}
