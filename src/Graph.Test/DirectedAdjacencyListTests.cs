using Graph.Indexes;

namespace Graph.Test
{
    public class DirectedAdjacencyListTests
        : AdjacencyIndexTests
    {
        protected override IAdjacencyIndex EmptyGraph()
        {
            return DirectedAdjacencyList.Empty;
        }
    }
}
