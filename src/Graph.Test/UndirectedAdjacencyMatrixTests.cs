using Graph.Indexes;

namespace Graph.Test
{
    public class UndirectedAdjacencyMatrixTests
        : AdjacencyIndexTests
    {
        protected override IAdjacencyIndex EmptyIndex()
        {
            return UndirectedAdjacencyMatrix.Empty;
        }
    }
}
