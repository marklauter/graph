namespace Graph.Indexes.Tests
{
    public class DirectedAdjacencyListTests
        : DirectedAdjacencyIndexTests
    {
        protected override IGraphIndex<int> EmptyIndex()
        {
            return DirectedAdjacencyList<int>.Empty();
        }
    }
}
