using Graph.Indexes;

namespace Graph.Traversals.Tests
{
    public sealed class FastDepthFirstPreOrderTraversalTests
        : TraversalTests
    {
        protected override ITraversal<int> CreateTraversal(IAdjacencyIndex<int> index)
        {
            return new FastDepthFirstPreOrderTraversal(index);
        }
    }
}
