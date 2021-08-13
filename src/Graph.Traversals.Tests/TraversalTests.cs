using Graph.Indexes;
using System;
using Xunit;

namespace Graph.Traversals.Tests
{
    public abstract class TraversalTests
    {
        protected abstract ITraversal<int> CreateTraversal(IGraphIndex<int> index);

        protected static IGraphIndex<int> CreateDirectedAdjacencyList()
        {
            return DirectedAdjacencyList<int>.Empty();
        }

        protected static IGraphIndex<int> CreateUndirectedAdjacencyList()
        {
            return UndirectedAdjacencyList<int>.Empty();
        }

        [Fact]
        public void Traversal_Directed_Depth_Succeeds()
        {
            var index = CreateDirectedAdjacencyList();
            index.Couple(0, 1);
            index.Couple(0, 2);
            index.Couple(1, 3);
            index.Couple(1, 4);
            index.Couple(2, 5);
            index.Couple(2, 6);

            var traversal = this.CreateTraversal(index);
            var depth = traversal.Depth(0);
            Assert.Equal(3, depth);

            index.Couple(3, 7);
            index.Couple(3, 8);

            depth = traversal.Depth(0);
            Assert.Equal(4, depth);

            index.Couple(8, 9);
            index.Couple(8, 10);

            depth = traversal.Depth(0);
            Assert.Equal(5, depth);

            depth = traversal.Depth(1);
            Assert.Equal(4, depth);
        }

        [Fact]
        public void Traversal_Undirected_Depth_Succeeds()
        {
            var index = CreateUndirectedAdjacencyList();
            index.Couple(0, 1);
            index.Couple(0, 2);
            index.Couple(1, 3);
            index.Couple(1, 4);
            index.Couple(2, 5);
            index.Couple(2, 6);

            var traversal = this.CreateTraversal(index);
            var depth = traversal.Depth(0);
            Assert.Equal(3, depth);

            index.Couple(3, 7);
            index.Couple(3, 8);

            depth = traversal.Depth(0);
            Assert.Equal(4, depth);

            index.Couple(8, 9);
            index.Couple(8, 10);

            depth = traversal.Depth(0);
            Assert.Equal(5, depth);

            depth = traversal.Depth(1);
            Assert.Equal(4, depth);
        }

        [Fact]
        public void Traversal_Directed_Traverse_Succeeds()
        {
            var size = 4;
            var index = CreateDirectedAdjacencyList();
            index.Couple(0, 1);
            index.Couple(0, 2);
            index.Couple(0, 3);
            index.Couple(1, 3);
            index.Couple(2, 3);
            index.Couple(3, 4);

            var traversal = this.CreateTraversal(index);
            var vertices = traversal.Traverse(0);
            Assert.NotEmpty(vertices);
            for (var i = size - 1; i >= 0; --i)
            {
                Assert.Contains(i, vertices);
            }
        }

        [Fact]
        public void Traversal_Undirected_Traverse_Succeeds()
        {
            var size = 4;
            var index = CreateUndirectedAdjacencyList();
            index.Couple(0, 1);
            index.Couple(0, 2);
            index.Couple(0, 3);
            index.Couple(1, 3);
            index.Couple(2, 3);
            index.Couple(3, 4);

            var traversal = this.CreateTraversal(index);
            var vertices = traversal.Traverse(0);
            Assert.NotEmpty(vertices);
            for (var i = size - 1; i >= 0; --i)
            {
                Assert.Contains(i, vertices);
            }
        }

        [Fact(Skip = "not implemented")]
        public void Traversal_Undirected_Traverse_With_Depth_Succeeds()
        {
            throw new NotImplementedException();
        }

        [Fact(Skip = "not implemented")]
        public void Traversal_Directed_Traverse_With_Depth_Succeeds()
        {
            throw new NotImplementedException();
        }
    }
}
