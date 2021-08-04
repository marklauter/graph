using Graph.Indexes;

namespace Graph.Test
{
    public class DirectedAdjacencyListTests
        : AdjacencyIndexTests
    {
        protected override IAdjacencyIndex<int> EmptyIndex()
        {
            return DirectedAdjacencyList.Empty;
        }
    }
}
