using Graph.Indexes;

namespace Graph.Test
{
    public class UndirectedAdjacencyMatrixTests
        : AdjacencyIndexTests
    {
        protected override IAdjacencyIndex EmptyGraph()
        {
            return UndirectedAdjacencyMatrix.Empty;
        }
    }
}
