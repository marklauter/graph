namespace Graphs.Indexes.Tests
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
