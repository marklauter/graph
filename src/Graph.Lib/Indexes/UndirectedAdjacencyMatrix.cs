using Graph.Graphs;

namespace Graph.Indexes
{
    public sealed class UndirectedAdjacencyMatrix
        : AdjacencyMatrix
    {
        public static IAdjacencyIndex<int> Empty { get; } = new UndirectedAdjacencyMatrix();

        public override GraphType Type => GraphType.Undirected;

        private UndirectedAdjacencyMatrix()
            : base()
        {
        }

        private UndirectedAdjacencyMatrix(AdjacencyMatrix other, int size)
            : base(other, size)
        {
        }

        public override bool Adjacent(int vertex1, int vertex2)
        {
            return this.Matrix[vertex1, vertex2] > 0
                && this.Matrix[vertex2, vertex1] > 0;
        }

        public override void Couple(int vertex1, int vertex2)
        {
            this.Matrix[vertex1, vertex2] = 1;
            this.Matrix[vertex2, vertex1] = 1;
        }

        public override void Decouple(int vertex1, int vertex2)
        {
            this.Matrix[vertex1, vertex2] = 0;
            this.Matrix[vertex2, vertex1] = 0;
        }

        public override IAdjacencyIndex<int> Resize(int size)
        {
            return new UndirectedAdjacencyMatrix(this, size);
        }
    }
}
