namespace Graph.Indexes.Tests
{
    public class UndirectedBinaryAdjacencyMatrixTests
        : UndirectedAdjacencyIndexTests
    {
        protected override IAdjacencyIndex<int> EmptyIndex()
        {
            return UndirectedBinaryAdjacencyMatrix.Empty();
        }
    }
}
