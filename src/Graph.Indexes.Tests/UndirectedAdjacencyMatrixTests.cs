namespace Graph.Indexes.Tests
{
    public class UndirectedAdjacencyMatrixTests
        : UndirectedAdjacencyIndexTests
    {
        protected override IAdjacencyIndex<int> EmptyIndex()
        {
            return UndirectedAdjacencyMatrix.Empty();
        }
    }
}
