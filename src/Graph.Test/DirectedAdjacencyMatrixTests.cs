using Graph.Indexes;

namespace Graph.Test
{
    public class DirectedAdjacencyMatrixTests
        : AdjacencyIndexTests
    {
        protected override IAdjacencyIndex EmptyGraph()
        {
            return DirectedAdjacencyMatrix.Empty;
        }
    }
}
