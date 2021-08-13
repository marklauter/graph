namespace Graph.Indexes.Tests
{
    public class UndirectedBinaryAdjacencyMatrixTests
        : UndirectedAdjacencyIndexTests
    {
        protected override IGraphIndex<int> EmptyIndex()
        {
            return UndirectedBinaryAdjacencyMatrix.Empty();
        }
    }
}
