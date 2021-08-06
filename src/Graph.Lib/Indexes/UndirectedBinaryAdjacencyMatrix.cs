using Graph.Graphs;
using System;

namespace Graph.Indexes
{
    public sealed class UndirectedBinaryAdjacencyMatrix
        : BinaryAdjacencyMatrix
    {
        public static IAdjacencyIndex<int> Empty()
        {
            return new UndirectedBinaryAdjacencyMatrix();
        }

        private UndirectedBinaryAdjacencyMatrix()
            : base()
        {
        }

        private UndirectedBinaryAdjacencyMatrix(BinaryAdjacencyMatrix other)
            : base(other)
        {
        }

        public override bool Adjacent(int source, int target)
        {
            return source < this.Size
                && target < this.Size
                && this.Read(source, target)
#pragma warning disable S2234 // Parameters should be passed in the correct order
                && this.Read(target, source);
#pragma warning restore S2234 // Parameters should be passed in the correct order
        }

        public override object Clone()
        {
            return new UndirectedBinaryAdjacencyMatrix(this);
        }

        public override bool Couple(int source, int target)
        {
            if (!this.Adjacent(source, target))
            {
                if (source >= this.Size || target >= this.Size)
                {
                    this.Grow(Math.Max(source, target));
                }

                this.Toggle(source, target);
#pragma warning disable S2234 // Parameters should be passed in the correct order
                this.Toggle(target, source);
#pragma warning restore S2234 // Parameters should be passed in the correct order
                return true;
            }

            return false;
        }

        public override bool Decouple(int source, int target)
        {
            if (this.Adjacent(source, target))
            {
                this.Toggle(source, target);
#pragma warning disable S2234 // Parameters should be passed in the correct order
                this.Toggle(target, source);
#pragma warning restore S2234 // Parameters should be passed in the correct order
                return true;
            }

            return false;
        }

        public override GraphType Type => GraphType.Undirected;
    }
}
