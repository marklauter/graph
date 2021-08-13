using Graph.Indexes;

namespace Graph.Traversals.Tests
{
    public sealed class DepthFirstPreOrderTraversalTests
        : TraversalTests
    {
        protected override ITraversal<int> CreateTraversal(IGraphIndex<int> index)
        {
            return new DepthFirstPreOrderTraversal<int>(index);
        }
    }
}
