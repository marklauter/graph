using Graphs.Indexes;

namespace Graphs.Traversals.Tests
{
    public sealed class DepthFirstPostOrderTraversalTests
        : TraversalTests
    {
        protected override ITraversal<int> CreateTraversal(IAdjacencyIndex<int> index)
        {
            return new DepthFirstPostOrderTraversal<int>(index);
        }
    }
}
