namespace Graphs.Indexes.Tests
{
    public class DirectedAdjacencyListTests
        : DirectedAdjacencyIndexTests
    {
        protected override IAdjacencyIndex<int> EmptyIndex()
        {
            return DirectedAdjacencyList<int>.Empty();
        }
    }
}
