using Graphs.Indexes;

namespace Graphs.Traversals.Tests
{
    public sealed class FastDepthFirstPostOrderTraversalTests
        : TraversalTests
    {
        protected override ITraversal<int> CreateTraversal(IAdjacencyIndex<int> index)
        {
            return new FastDepthFirstPostOrderTraversal(index);
        }
    }
}
