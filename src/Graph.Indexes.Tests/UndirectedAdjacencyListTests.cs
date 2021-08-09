namespace Graph.Indexes.Tests
{
    public class UndirectedAdjacencyListTests
        : UndirectedAdjacencyIndexTests
    {
        protected override IAdjacencyIndex<int> EmptyIndex()
        {
            return UndirectedAdjacencyList<int>.Empty();
        }
    }
}
