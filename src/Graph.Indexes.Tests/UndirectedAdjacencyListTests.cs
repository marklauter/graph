namespace Graph.Indexes.Tests
{
    public class UndirectedAdjacencyListTests
        : UndirectedAdjacencyIndexTests
    {
        protected override IGraphIndex<int> EmptyIndex()
        {
            return UndirectedAdjacencyList<int>.Empty();
        }
    }
}
