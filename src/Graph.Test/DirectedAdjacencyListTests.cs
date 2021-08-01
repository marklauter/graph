using Graph.Sets;

namespace Graph.Test
{
    public class DirectedAdjacencyListTests
        : GraphTests
    {
        protected override IGraph EmptyGraph()
        {
            return DirectedAdjacencyList.Empty;
        }
    }
}
