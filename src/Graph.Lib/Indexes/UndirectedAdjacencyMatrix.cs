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

        public override bool Adjacent(int vertex1, int vertex2)
        {
            return vertex1 < this.Size
                && vertex2 < this.Size
                && this.Read(vertex1, vertex2)
                && this.Read(vertex2, vertex1);
        }

        private bool Read(int x, int y)
        {
            var col = Math.DivRem(y, sizeof(ulong), out var bit);
            var row = this.Matrix[x, col];
            return (row & ((ulong)1 >> (bit - 1))) != 0;
        }

        private void Write(int x, int y)
        {
            var col = Math.DivRem(y, sizeof(ulong), out var bit);
            var row = this.Matrix[x, col];
            this.Matrix[x, col] = row | ((ulong)1 >> (bit - 1));
        }

        public override object Clone()
        {
            return new UndirectedBinaryAdjacencyMatrix(this);
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
                this.Matrix[vertex2, vertex1] = true;
                return true;
            }

            return false;
        }

        public override bool Decouple(int vertex1, int vertex2)
        {
            if (this.Adjacent(vertex1, vertex2))
            {
                this.Matrix[vertex1, vertex2] = false;
                this.Matrix[vertex2, vertex1] = false;
                return true;
            }

            return false;
        }

        public override GraphType Type => GraphType.Undirected;
    }


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

        public override bool Adjacent(int vertex1, int vertex2)
        {
            return vertex1 < this.Size
                && vertex2 < this.Size
                && this.Matrix[vertex1, vertex2]
                && this.Matrix[vertex2, vertex1];
        }

        public override object Clone()
        {
            return new UndirectedAdjacencyMatrix(this);
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
                this.Matrix[vertex2, vertex1] = true;
                return true;
            }

            return false;
        }

        public override bool Decouple(int vertex1, int vertex2)
        {
            if (this.Adjacent(vertex1, vertex2))
            {
                this.Matrix[vertex1, vertex2] = false;
                this.Matrix[vertex2, vertex1] = false;
                return true;
            }

            return false;
        }

        public override GraphType Type => GraphType.Undirected;
    }
}
