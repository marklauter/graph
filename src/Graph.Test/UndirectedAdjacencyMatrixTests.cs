using Graph.Sets;

namespace Graph.Test
{
    public class UndirectedAdjacencyMatrixTests
        : GraphTests
    {
        protected override IGraph EmptyGraph()
        {
            return UndirectedAdjacencyMatrix.Empty;
        }
    }
}
