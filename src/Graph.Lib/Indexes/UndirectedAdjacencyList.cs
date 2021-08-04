using Graph.Graphs;

namespace Graph.Indexes
{
    public sealed class UndirectedAdjacencyList
        : AdjacencyList
    {
        public static IAdjacencyIndex<int> Empty { get; } = new UndirectedAdjacencyList();

        public override GraphType Type => GraphType.Undirected;

        private UndirectedAdjacencyList()
            : base()
        {
        }

        private UndirectedAdjacencyList(AdjacencyList other, int size)
            : base(other, size)
        {
        }

        public override bool Adjacent(int vertex1, int vertex2)
        {
            return this.Index[vertex1].Contains(vertex2)
                && this.Index[vertex2].Contains(vertex1);
        }

        public override void Connect(int vertex1, int vertex2)
        {
            this.Index[vertex1].Add(vertex2);
            this.Index[vertex2].Add(vertex1);
        }

        public override void Disconnect(int vertex1, int vertex2)
        {
            this.Index[vertex1].Remove(vertex2);
            this.Index[vertex2].Remove(vertex1);
        }

        public override IAdjacencyIndex<int> Resize(int size)
        {
            return new UndirectedAdjacencyList(this, size);
        }
    }

}
