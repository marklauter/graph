using Graph.Indexes;

namespace Graph.Test
{
    public class DirectedAdjacencyMatrixTests
        : AdjacencyIndexTests
    {
        protected override IAdjacencyIndex EmptyIndex()
        {
            return DirectedAdjacencyMatrix.Empty;
        }
    }
}
