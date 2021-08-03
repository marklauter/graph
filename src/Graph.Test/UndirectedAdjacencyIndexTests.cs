using Graph.Indexes;

namespace Graph.Test
{
    public class UndirectedAdjacencyListTests
        : AdjacencyIndexTests
    {
        protected override IAdjacencyIndex EmptyIndex()
        {
            return UndirectedAdjacencyList.Empty;
        }
    }
}
