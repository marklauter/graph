using Graph.Indexes;

namespace Graph.Test
{
    public class UndirectedAdjacencyMatrixTests
        : AdjacencyIndexTests
    {
        protected override IAdjacencyIndex<int> EmptyIndex()
        {
            return UndirectedAdjacencyMatrix.Empty;
        }
    }
}
