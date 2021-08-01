using Graph.Sets;

namespace Graph.Test
{
    public class UndirectedAdjacencyListTests
        : GraphTests
    {
        protected override IGraph EmptyGraph()
        {
            return UnDirectedAdjacencyList.Empty;
        }
    }
}
