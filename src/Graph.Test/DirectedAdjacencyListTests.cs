using Graph.Indexes;

namespace Graph.Test
{
    public class DirectedAdjacencyListTests
        : AdjacencyIndexTests
    {
        protected override IAdjacencyIndex EmptyIndex()
        {
            return DirectedAdjacencyList.Empty;
        }
    }
}
