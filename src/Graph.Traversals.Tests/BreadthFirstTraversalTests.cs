using Graphs.Indexes;

namespace Graphs.Traversals.Tests
{
    public sealed class BreadthFirstTraversalTests
        : TraversalTests
    {
        protected override ITraversal<int> CreateTraversal(IAdjacencyIndex<int> index)
        {
            return new BreadthFirstTraversal<int>(index);
        }
    }
}
