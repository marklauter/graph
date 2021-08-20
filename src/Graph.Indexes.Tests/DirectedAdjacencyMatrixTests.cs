namespace Graphs.Indexes.Tests
{
    public class DirectedAdjacencyMatrixTests
        : DirectedAdjacencyIndexTests
    {
        protected override IAdjacencyIndex<int> EmptyIndex()
        {
            return DirectedAdjacencyMatrix.Empty();
        }
    }
}
