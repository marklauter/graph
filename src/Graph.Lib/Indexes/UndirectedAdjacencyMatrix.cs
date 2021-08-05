using Graph.Graphs;

namespace Graph.Indexes
{
    public sealed class UndirectedAdjacencyMatrix
        : AdjacencyMatrix
    {
        public static IAdjacencyIndex<int> Empty { get; } = new UndirectedAdjacencyMatrix();

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
            return this.Matrix[vertex1, vertex2]
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
                this.Grow();
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
            if (this.Matrix[vertex1, vertex2])
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
