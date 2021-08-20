using Graph.Indexes;

namespace Graph.Traversals.Tests
{
    public sealed class BreadthFirstPreOrderTraversalTests
        : TraversalTests
    {
        protected override ITraversal<int> CreateTraversal(IAdjacencyIndex<int> index)
        {
            return new BreadthFirstPreOrderTraversal<int>(index);
        }
    }
}
