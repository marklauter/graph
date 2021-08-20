namespace Graphs.Indexes.Tests
{
    public class UndirectedBinaryAdjacencyMatrixTests
        : UndirectedAdjacencyIndexTests
    {
        protected override IAdjacencyIndex<int> EmptyIndex()
        {
            return BinaryUndirectedAdjacencyMatrix.Empty();
        }
    }
}
