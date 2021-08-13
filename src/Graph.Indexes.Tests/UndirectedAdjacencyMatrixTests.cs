namespace Graph.Indexes.Tests
{
    public class UndirectedAdjacencyMatrixTests
        : UndirectedAdjacencyIndexTests
    {
        protected override IGraphIndex<int> EmptyIndex()
        {
            return UndirectedAdjacencyMatrix.Empty();
        }
    }
}
