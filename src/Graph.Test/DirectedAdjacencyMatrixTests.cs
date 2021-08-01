using Graph.Sets;

namespace Graph.Test
{
    public class DirectedAdjacencyMatrixTests
        : GraphTests
    {
        protected override IGraph EmptyGraph()
        {
            return DirectedAdjacencyMatrix.Empty;
        }
    }
}
