namespace Graph.Indexes.Tests
{
    public class DirectedAdjacencyMatrixTests
        : DirectedAdjacencyIndexTests
    {
        protected override IGraphIndex<int> EmptyIndex()
        {
            return DirectedAdjacencyMatrix.Empty();
        }
    }
}
