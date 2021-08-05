using Graph.Graphs;
using Graph.Indexes;
using Xunit;

namespace Graph.Test
{
    public abstract class AdjacencyIndexTests
    {
        protected abstract IAdjacencyIndex<int> EmptyIndex();

        [Fact]
        public void Graph_Empty_Succeeds()
        {
            var index = this.EmptyIndex();
            Assert.Equal(0, index.Size);
        }

        [Fact]
        public void Graph_Resize_Succeeds()
        {
            var size = 13;
            var index = this.EmptyIndex().Resize(size);
            Assert.Equal(size, index.Size);
        }

        [Fact]
        public void Graph_Connect_Succeeds()
        {
            var size = 2;
            var index = this.EmptyIndex().Resize(size);
            Assert.Equal(size, index.Size);

            index.Couple(0, 1);

            if (index.Type == GraphType.Undirected)
            {
                Assert.False(index.Adjacent(0, 0));
                Assert.True(index.Adjacent(0, 1));
                Assert.True(index.Adjacent(1, 0));
                Assert.False(index.Adjacent(1, 1));
            }
            else
            {
                Assert.False(index.Adjacent(0, 0));
                Assert.True(index.Adjacent(0, 1));
                Assert.False(index.Adjacent(1, 0));
                Assert.False(index.Adjacent(1, 1));
            }
        }

        [Fact]
        public void Graph_Connect_Twice_Succeeds()
        {
            var size = 2;
            var index = this.EmptyIndex().Resize(size);
            Assert.Equal(size, index.Size);

            index.Couple(0, 1);
            index.Couple(0, 1);

            if (index.Type == GraphType.Undirected)
            {
                Assert.False(index.Adjacent(0, 0));
                Assert.True(index.Adjacent(0, 1));
                Assert.True(index.Adjacent(1, 0));
                Assert.False(index.Adjacent(1, 1));
            }
            else
            {
                Assert.False(index.Adjacent(0, 0));
                Assert.True(index.Adjacent(0, 1));
                Assert.False(index.Adjacent(1, 0));
                Assert.False(index.Adjacent(1, 1));
            }

            Assert.Single(index.Neighbors(0));
        }

        [Fact]
        public void Graph_Disconnect_Succeeds()
        {
            var size = 2;
            var index = this.EmptyIndex().Resize(size);
            Assert.Equal(size, index.Size);

            index.Couple(0, 1);

            if (index.Type == GraphType.Undirected)
            {
                Assert.True(index.Adjacent(0, 1));
                Assert.True(index.Adjacent(1, 0));
            }
            else
            {
                Assert.True(index.Adjacent(0, 1));
                Assert.False(index.Adjacent(1, 0));
            }

            index.Decouple(0, 1);
            Assert.False(index.Adjacent(0, 1));
            Assert.False(index.Adjacent(1, 0));

            index.Couple(0, 1);
            index.Decouple(1, 0);

            if (index.Type == GraphType.Undirected)
            {
                Assert.False(index.Adjacent(0, 1));
                Assert.False(index.Adjacent(1, 0));
            }
            else
            {
                Assert.True(index.Adjacent(0, 1));
                Assert.False(index.Adjacent(1, 0));
            }
        }

        [Fact]
        public void Graph_Resize_Is_NonDestructive()
        {
            var size = 2;
            var index = this.EmptyIndex().Resize(size);
            Assert.Equal(size, index.Size);
            index.Couple(0, 1);
            if (index.Type == GraphType.Undirected)
            {
                Assert.False(index.Adjacent(0, 0));
                Assert.True(index.Adjacent(0, 1));
                Assert.True(index.Adjacent(1, 0));
                Assert.False(index.Adjacent(1, 1));
            }
            else
            {
                Assert.False(index.Adjacent(0, 0));
                Assert.True(index.Adjacent(0, 1));
                Assert.False(index.Adjacent(1, 0));
                Assert.False(index.Adjacent(1, 1));
            }

            size = 3;
            index = index.Resize(size);
            Assert.Equal(size, index.Size);

            if (index.Type == GraphType.Undirected)
            {
                Assert.False(index.Adjacent(0, 0));
                Assert.True(index.Adjacent(0, 1));
                Assert.True(index.Adjacent(1, 0));
                Assert.False(index.Adjacent(1, 1));
                Assert.False(index.Adjacent(0, 2));
                Assert.False(index.Adjacent(1, 2));
                Assert.False(index.Adjacent(2, 1));
                Assert.False(index.Adjacent(2, 0));
            }
            else
            {
                Assert.False(index.Adjacent(0, 0));
                Assert.True(index.Adjacent(0, 1));
                Assert.False(index.Adjacent(1, 0));
                Assert.False(index.Adjacent(1, 1));
                Assert.False(index.Adjacent(0, 2));
                Assert.False(index.Adjacent(1, 2));
                Assert.False(index.Adjacent(2, 1));
                Assert.False(index.Adjacent(2, 0));
            }
        }

        [Fact]
        public void Graph_DepthFirstSearchPreOrder_Succeeds()
        {
            var size = 5;
            var index = this.EmptyIndex().Resize(size);
            Assert.Equal(size, index.Size);
            index.Couple(0, 1);
            index.Couple(0, 2);
            index.Couple(0, 3);
            index.Couple(1, 3);
            index.Couple(2, 3);
            index.Couple(3, 4);

            var vertices = index.DepthFirstSearchPreOrder(0);
            Assert.NotEmpty(vertices);
            for (var i = size - 1; i >= 0; --i)
            {
                Assert.Contains(i, vertices);
            }
        }

        [Fact]
        public void Graph_DepthFirstSearchPostOrder_Succeeds()
        {
            var size = 5;
            var index = this.EmptyIndex().Resize(size);
            Assert.Equal(size, index.Size);

            index.Couple(0, 1);
            index.Couple(0, 2);
            index.Couple(0, 3);
            index.Couple(1, 3);
            index.Couple(2, 3);
            index.Couple(3, 4);

            var vertices = index.DepthFirstSearchPostOrder(0);
            Assert.NotEmpty(vertices);
            for (var i = size - 1; i >= 0; --i)
            {
                Assert.Contains(i, vertices);
            }
        }

        [Fact]
        public void Graph_Degrees_Succeeds()
        {
            var size = 3;
            var index = this.EmptyIndex().Resize(size);
            Assert.Equal(size, index.Size);

            index.Couple(0, 1);
            index.Couple(1, 2);

            if (index.Type == GraphType.Undirected)
            {
                Assert.Equal(1, index.Degree(0));
                Assert.Equal(2, index.Degree(1));
                Assert.Equal(1, index.Degree(2));
            }
            else
            {
                Assert.Equal(1, index.Degree(0));
                Assert.Equal(1, index.Degree(1));
                Assert.Equal(0, index.Degree(2));
            }
        }

        [Fact]
        public void Graph_Neighbors_Succeeds()
        {
            var size = 3;
            var index = this.EmptyIndex().Resize(size);
            Assert.Equal(size, index.Size);

            index.Couple(0, 1);
            index.Couple(1, 2);

            var neighbors = index.Neighbors(1);
            if (index.Type == GraphType.Undirected)
            {
                Assert.Contains(0, neighbors);
                Assert.Contains(2, neighbors);
            }
            else
            {
                Assert.DoesNotContain(0, neighbors);
                Assert.Contains(2, neighbors);
            }
        }

        [Fact]
        public void Graph_Clone_Succeeds()
        {
            var size = 3;
            var index = this.EmptyIndex().Resize(size);
            Assert.Equal(size, index.Size);
            index.Couple(0, 1);
            index.Couple(1, 2);

            if (index.Type == GraphType.Undirected)
            {
                Assert.False(index.Adjacent(0, 0));
                Assert.True(index.Adjacent(0, 1));
                Assert.False(index.Adjacent(0, 2));

                Assert.True(index.Adjacent(1, 0));
                Assert.False(index.Adjacent(1, 1));
                Assert.True(index.Adjacent(1, 2));

                Assert.False(index.Adjacent(2, 0));
                Assert.True(index.Adjacent(2, 1));
                Assert.False(index.Adjacent(2, 2));
            }
            else
            {
                Assert.False(index.Adjacent(0, 0));
                Assert.True(index.Adjacent(0, 1));
                Assert.False(index.Adjacent(0, 2));

                Assert.False(index.Adjacent(1, 0));
                Assert.False(index.Adjacent(1, 1));
                Assert.True(index.Adjacent(1, 2));

                Assert.False(index.Adjacent(2, 0));
                Assert.False(index.Adjacent(2, 1));
                Assert.False(index.Adjacent(2, 2));
            }

            var clone = index.Clone();

            if (index.Type == GraphType.Undirected)
            {
                Assert.False(clone.Adjacent(0, 0));
                Assert.True(clone.Adjacent(0, 1));
                Assert.False(clone.Adjacent(0, 2));

                Assert.True(clone.Adjacent(1, 0));
                Assert.False(clone.Adjacent(1, 1));
                Assert.True(clone.Adjacent(1, 2));

                Assert.False(clone.Adjacent(2, 0));
                Assert.True(clone.Adjacent(2, 1));
                Assert.False(clone.Adjacent(2, 2));
            }
            else
            {
                Assert.False(clone.Adjacent(0, 0));
                Assert.True(clone.Adjacent(0, 1));
                Assert.False(clone.Adjacent(0, 2));

                Assert.False(clone.Adjacent(1, 0));
                Assert.False(clone.Adjacent(1, 1));
                Assert.True(clone.Adjacent(1, 2));

                Assert.False(clone.Adjacent(2, 0));
                Assert.False(clone.Adjacent(2, 1));
                Assert.False(clone.Adjacent(2, 2));
            }
        }
    }
}
