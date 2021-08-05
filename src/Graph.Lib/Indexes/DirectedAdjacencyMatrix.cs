using Graph.Graphs;
using System;

namespace Graph.Indexes
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

        public DirectedAdjacencyMatrix(AdjacencyMatrix other)
            : base(other)
        {
        }

        public override bool Adjacent(int vertex1, int vertex2)
        {
            return vertex1 < this.Size
                && vertex2 < this.Size
                && this.Matrix[vertex1, vertex2];
        }

        public override object Clone()
        {
            return new DirectedAdjacencyMatrix(this);
        }

        public override bool Couple(int vertex1, int vertex2)
        {
            if (vertex1 >= this.Size || vertex2 >= this.Size)
            {
                this.Grow(Math.Max(vertex1, vertex2));
            }

            if (!this.Matrix[vertex1, vertex2])
            {
                this.Matrix[vertex1, vertex2] = true;
                return true;
            }

            return false;
        }

        public override bool Decouple(int vertex1, int vertex2)
        {
            if (this.Adjacent(vertex1, vertex2))
            {
                this.Matrix[vertex1, vertex2] = false;
                return true;
            }

            return false;
        }

        public override GraphType Type => GraphType.Directed;
    }
}
