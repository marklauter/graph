using Graph.Indexes;
using Graph.Graphs;
using Xunit;

namespace Graph.Test
{
    public abstract class AdjacencyIndexTests
    {
        protected abstract IAdjacencyIndex EmptyGraph();

        [Fact]
        public void Graph_Empty_Succeeds()
        {
            var graph = this.EmptyGraph();
            Assert.Equal(0, graph.Size);
        }

        [Fact]
        public void Graph_Resize_Succeeds()
        {
            var size = 13;
            var graph = this.EmptyGraph().Resize(size);
            Assert.Equal(size, graph.Size);
        }

        [Fact]
        public void Graph_Connect_Succeeds()
        {
            var size = 2;
            var graph = this.EmptyGraph().Resize(size);
            Assert.Equal(size, graph.Size);

            graph.Connect(0, 1);

            if (graph.Type == GraphType.Undirected)
            {
                Assert.False(graph.Adjacent(0, 0));
                Assert.True(graph.Adjacent(0, 1));
                Assert.True(graph.Adjacent(1, 0));
                Assert.False(graph.Adjacent(1, 1));
            }
            else
            {
                Assert.False(graph.Adjacent(0, 0));
                Assert.True(graph.Adjacent(0, 1));
                Assert.False(graph.Adjacent(1, 0));
                Assert.False(graph.Adjacent(1, 1));
            }
        }

        [Fact]
        public void Graph_Connect_Twice_Succeeds()
        {
            var size = 2;
            var graph = this.EmptyGraph().Resize(size);
            Assert.Equal(size, graph.Size);

            graph.Connect(0, 1);
            graph.Connect(0, 1);

            if (graph.Type == GraphType.Undirected)
            {
                Assert.False(graph.Adjacent(0, 0));
                Assert.True(graph.Adjacent(0, 1));
                Assert.True(graph.Adjacent(1, 0));
                Assert.False(graph.Adjacent(1, 1));
            }
            else
            {
                Assert.False(graph.Adjacent(0, 0));
                Assert.True(graph.Adjacent(0, 1));
                Assert.False(graph.Adjacent(1, 0));
                Assert.False(graph.Adjacent(1, 1));
            }

            Assert.Single(graph.Neighbors(0));
        }

        [Fact]
        public void Graph_Disconnect_Succeeds()
        {
            var size = 2;
            var graph = this.EmptyGraph().Resize(size);
            Assert.Equal(size, graph.Size);

            graph.Connect(0, 1);

            if (graph.Type == GraphType.Undirected)
            {
                Assert.True(graph.Adjacent(0, 1));
                Assert.True(graph.Adjacent(1, 0));
            }
            else
            {
                Assert.True(graph.Adjacent(0, 1));
                Assert.False(graph.Adjacent(1, 0));
            }

            graph.Disconnect(0, 1);
            Assert.False(graph.Adjacent(0, 1));
            Assert.False(graph.Adjacent(1, 0));

            graph.Connect(0, 1);
            graph.Disconnect(1, 0);

            if (graph.Type == GraphType.Undirected)
            {
                Assert.False(graph.Adjacent(0, 1));
                Assert.False(graph.Adjacent(1, 0));
            }
            else
            {
                Assert.True(graph.Adjacent(0, 1));
                Assert.False(graph.Adjacent(1, 0));
            }
        }

        [Fact]
        public void Graph_Resize_Is_NonDestructive()
        {
            var size = 2;
            var graph = this.EmptyGraph().Resize(size);
            Assert.Equal(size, graph.Size);
            graph.Connect(0, 1);
            if (graph.Type == GraphType.Undirected)
            {
                Assert.False(graph.Adjacent(0, 0));
                Assert.True(graph.Adjacent(0, 1));
                Assert.True(graph.Adjacent(1, 0));
                Assert.False(graph.Adjacent(1, 1));
            }
            else
            {
                Assert.False(graph.Adjacent(0, 0));
                Assert.True(graph.Adjacent(0, 1));
                Assert.False(graph.Adjacent(1, 0));
                Assert.False(graph.Adjacent(1, 1));
            }

            size = 3;
            graph = graph.Resize(size);
            Assert.Equal(size, graph.Size);

            if (graph.Type == GraphType.Undirected)
            {
                Assert.False(graph.Adjacent(0, 0));
                Assert.True(graph.Adjacent(0, 1));
                Assert.True(graph.Adjacent(1, 0));
                Assert.False(graph.Adjacent(1, 1));
                Assert.False(graph.Adjacent(0, 2));
                Assert.False(graph.Adjacent(1, 2));
                Assert.False(graph.Adjacent(2, 1));
                Assert.False(graph.Adjacent(2, 0));
            }
            else
            {
                Assert.False(graph.Adjacent(0, 0));
                Assert.True(graph.Adjacent(0, 1));
                Assert.False(graph.Adjacent(1, 0));
                Assert.False(graph.Adjacent(1, 1));
                Assert.False(graph.Adjacent(0, 2));
                Assert.False(graph.Adjacent(1, 2));
                Assert.False(graph.Adjacent(2, 1));
                Assert.False(graph.Adjacent(2, 0));
            }
        }

        [Fact]
        public void Graph_DepthFirstSearchPreOrder_Succeeds()
        {
            var size = 5;
            var graph = this.EmptyGraph().Resize(size);
            Assert.Equal(size, graph.Size);
            graph.Connect(0, 1);
            graph.Connect(0, 2);
            graph.Connect(0, 3);
            graph.Connect(1, 3);
            graph.Connect(2, 3);
            graph.Connect(3, 4);

            var vertices = graph.DepthFirstSearchPreOrder(0);
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
            var graph = this.EmptyGraph().Resize(size);
            Assert.Equal(size, graph.Size);

            graph.Connect(0, 1);
            graph.Connect(0, 2);
            graph.Connect(0, 3);
            graph.Connect(1, 3);
            graph.Connect(2, 3);
            graph.Connect(3, 4);

            var vertices = graph.DepthFirstSearchPostOrder(0);
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
            var graph = this.EmptyGraph().Resize(size);
            Assert.Equal(size, graph.Size);

            graph.Connect(0, 1);
            graph.Connect(1, 2);

            if (graph.Type == GraphType.Undirected)
            {
                Assert.Equal(1, graph.Degree(0));
                Assert.Equal(2, graph.Degree(1));
                Assert.Equal(1, graph.Degree(2));
            }
            else
            {
                Assert.Equal(1, graph.Degree(0));
                Assert.Equal(1, graph.Degree(1));
                Assert.Equal(0, graph.Degree(2));
            }
        }

        [Fact]
        public void Graph_Neighbors_Succeeds()
        {
            var size = 3;
            var graph = this.EmptyGraph().Resize(size);
            Assert.Equal(size, graph.Size);

            graph.Connect(0, 1);
            graph.Connect(1, 2);

            var neighbors = graph.Neighbors(1);
            if (graph.Type == GraphType.Undirected)
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
            var graph = this.EmptyGraph().Resize(size);
            Assert.Equal(size, graph.Size);
            graph.Connect(0, 1);
            graph.Connect(1, 2);

            if (graph.Type == GraphType.Undirected)
            {
                Assert.False(graph.Adjacent(0, 0));
                Assert.True(graph.Adjacent(0, 1));
                Assert.False(graph.Adjacent(0, 2));

                Assert.True(graph.Adjacent(1, 0));
                Assert.False(graph.Adjacent(1, 1));
                Assert.True(graph.Adjacent(1, 2));

                Assert.False(graph.Adjacent(2, 0));
                Assert.True(graph.Adjacent(2, 1));
                Assert.False(graph.Adjacent(2, 2));
            }
            else
            {
                Assert.False(graph.Adjacent(0, 0));
                Assert.True(graph.Adjacent(0, 1));
                Assert.False(graph.Adjacent(0, 2));

                Assert.False(graph.Adjacent(1, 0));
                Assert.False(graph.Adjacent(1, 1));
                Assert.True(graph.Adjacent(1, 2));

                Assert.False(graph.Adjacent(2, 0));
                Assert.False(graph.Adjacent(2, 1));
                Assert.False(graph.Adjacent(2, 2));
            }

            var clone = graph.Clone();

            if (graph.Type == GraphType.Undirected)
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
