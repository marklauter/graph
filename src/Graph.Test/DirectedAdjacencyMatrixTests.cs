using Graph.Indexes;

namespace Graph.Test
{
    public class DirectedAdjacencyMatrixTests
        : AdjacencyIndexTests
    {
        protected override IAdjacencyIndex<int> EmptyIndex()
        {
            return DirectedAdjacencyMatrix.Empty;
        }
    }
}
