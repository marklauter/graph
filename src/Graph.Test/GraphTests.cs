using Graph.Graphs;
using Graph.Indexes;
using System;
using Xunit;

namespace Graph.Test
{
    public class UndirectedListGraphTests
        : GraphTests
    {
        protected override IGraph<Guid> CreateGraph(Guid[] vertices)
        {
            var graph = new MemoryGraph<Guid>(UndirectedAdjacencyList.Empty);
            graph.AddRange(vertices);
            return graph;
        }
    }

    public class DirectedListGraphTests
        : GraphTests
    {
        protected override IGraph<Guid> CreateGraph(Guid[] vertices)
        {
            var graph = new MemoryGraph<Guid>(DirectedAdjacencyList.Empty);
            graph.AddRange(vertices);
            return graph;
        }
    }

    public class UndirectedMatrixGraphTests
    : GraphTests
    {
        protected override IGraph<Guid> CreateGraph(Guid[] vertices)
        {
            var graph = new MemoryGraph<Guid>(UndirectedAdjacencyMatrix.Empty);
            graph.AddRange(vertices);
            return graph;
        }
    }

    public class DirectedMatrixGraphTests
        : GraphTests
    {
        protected override IGraph<Guid> CreateGraph(Guid[] vertices)
        {
            var graph = new MemoryGraph<Guid>(DirectedAdjacencyMatrix.Empty);
            graph.AddRange(vertices);
            return graph;
        }
    }


    public abstract class GraphTests
    {
        protected abstract IGraph<Guid> CreateGraph(Guid[] vertices);

        private static Guid[] GenerateVertices(int size)
        {
            var vertices = new Guid[size];

            for (var i = size - 1; i >= 0; --i)
            {
                vertices[i] = Guid.NewGuid();
            }

            return vertices;
        }

        [Fact]
        public void Graph_Empty_Succeeds()
        {
            var vertices = GenerateVertices(0);
            var graph = this.CreateGraph(vertices);
            Assert.Equal(0, graph.Size);
        }

        [Fact]
        public void Graph_Resize_Succeeds()
        {
            var size = 13;
            var vertices = GenerateVertices(size);
            var graph = this.CreateGraph(vertices);
            Assert.Equal(size, graph.Size);
        }

        [Fact]
        public void Graph_Connect_Succeeds()
        {
            var size = 2;
            var vertices = GenerateVertices(size);
            var graph = this.CreateGraph(vertices);
            Assert.Equal(size, graph.Size);

            graph.Connect(vertices[0], vertices[1]);

            if (graph.Type == GraphType.Undirected)
            {
                Assert.False(graph.Adjacent(vertices[0], vertices[0]));
                Assert.True(graph.Adjacent(vertices[0], vertices[1]));
                Assert.True(graph.Adjacent(vertices[1], vertices[0]));
                Assert.False(graph.Adjacent(vertices[1], vertices[1]));
            }
            else
            {
                Assert.False(graph.Adjacent(vertices[0], vertices[0]));
                Assert.True(graph.Adjacent(vertices[0], vertices[1]));
                Assert.False(graph.Adjacent(vertices[1], vertices[0]));
                Assert.False(graph.Adjacent(vertices[1], vertices[1]));
            }
        }

        [Fact]
        public void Graph_Connect_Twice_Succeeds()
        {
            var size = 2;
            var vertices = GenerateVertices(size);
            var graph = this.CreateGraph(vertices);
            Assert.Equal(size, graph.Size);

            graph.Connect(vertices[0], vertices[1]);
            graph.Connect(vertices[0], vertices[1]);

            if (graph.Type == GraphType.Undirected)
            {
                Assert.False(graph.Adjacent(vertices[0], vertices[0]));
                Assert.True(graph.Adjacent(vertices[0], vertices[1]));
                Assert.True(graph.Adjacent(vertices[1], vertices[0]));
                Assert.False(graph.Adjacent(vertices[1], vertices[1]));
            }
            else
            {
                Assert.False(graph.Adjacent(vertices[0], vertices[0]));
                Assert.True(graph.Adjacent(vertices[0], vertices[1]));
                Assert.False(graph.Adjacent(vertices[1], vertices[0]));
                Assert.False(graph.Adjacent(vertices[1], vertices[1]));
            }

            Assert.Single(graph.Neighbors(vertices[0]));
        }

        [Fact]
        public void Graph_Disconnect_Succeeds()
        {
            var size = 2;
            var vertices = GenerateVertices(size);
            var graph = this.CreateGraph(vertices);
            Assert.Equal(size, graph.Size);

            graph.Connect(vertices[0], vertices[1]);

            if (graph.Type == GraphType.Undirected)
            {
                Assert.True(graph.Adjacent(vertices[0], vertices[1]));
                Assert.True(graph.Adjacent(vertices[1], vertices[0]));
            }
            else
            {
                Assert.True(graph.Adjacent(vertices[0], vertices[1]));
                Assert.False(graph.Adjacent(vertices[1], vertices[0]));
            }

            graph.Disconnect(vertices[0], vertices[1]);
            Assert.False(graph.Adjacent(vertices[0], vertices[1]));
            Assert.False(graph.Adjacent(vertices[1], vertices[0]));

            graph.Connect(vertices[0], vertices[1]);
            graph.Disconnect(vertices[1], vertices[0]);

            if (graph.Type == GraphType.Undirected)
            {
                Assert.False(graph.Adjacent(vertices[0], vertices[1]));
                Assert.False(graph.Adjacent(vertices[1], vertices[0]));
            }
            else
            {
                Assert.True(graph.Adjacent(vertices[0], vertices[1]));
                Assert.False(graph.Adjacent(vertices[1], vertices[0]));
            }
        }

        [Fact]
        public void Graph_Resize_Is_NonDestructive()
        {
            var size = 2;
            var vertices = GenerateVertices(size);
            var graph = this.CreateGraph(vertices);
            Assert.Equal(size, graph.Size);
            graph.Connect(vertices[0], vertices[1]);
            if (graph.Type == GraphType.Undirected)
            {
                Assert.False(graph.Adjacent(vertices[0], vertices[0]));
                Assert.True(graph.Adjacent(vertices[0], vertices[1]));
                Assert.True(graph.Adjacent(vertices[1], vertices[0]));
                Assert.False(graph.Adjacent(vertices[1], vertices[1]));
            }
            else
            {
                Assert.False(graph.Adjacent(vertices[0], vertices[0]));
                Assert.True(graph.Adjacent(vertices[0], vertices[1]));
                Assert.False(graph.Adjacent(vertices[1], vertices[0]));
                Assert.False(graph.Adjacent(vertices[1], vertices[1]));
            }

            size = 3;
            vertices = new Guid[] { vertices[0], vertices[1], Guid.NewGuid() };

            graph = graph.Clone();
            graph.Add(vertices[2]);

            Assert.Equal(size, graph.Size);

            if (graph.Type == GraphType.Undirected)
            {
                Assert.False(graph.Adjacent(vertices[0], vertices[0]));
                Assert.True(graph.Adjacent(vertices[0], vertices[1]));
                Assert.True(graph.Adjacent(vertices[1], vertices[0]));
                Assert.False(graph.Adjacent(vertices[1], vertices[1]));
                Assert.False(graph.Adjacent(vertices[0], vertices[2]));
                Assert.False(graph.Adjacent(vertices[1], vertices[2]));
                Assert.False(graph.Adjacent(vertices[2], vertices[1]));
                Assert.False(graph.Adjacent(vertices[2], vertices[0]));
            }
            else
            {
                Assert.False(graph.Adjacent(vertices[0], vertices[0]));
                Assert.True(graph.Adjacent(vertices[0], vertices[1]));
                Assert.False(graph.Adjacent(vertices[1], vertices[0]));
                Assert.False(graph.Adjacent(vertices[1], vertices[1]));
                Assert.False(graph.Adjacent(vertices[0], vertices[2]));
                Assert.False(graph.Adjacent(vertices[1], vertices[2]));
                Assert.False(graph.Adjacent(vertices[2], vertices[1]));
                Assert.False(graph.Adjacent(vertices[2], vertices[0]));
            }
        }

        [Fact]
        public void Graph_DepthFirstSearchPreOrder_Succeeds()
        {
            var size = 5;
            var vertices = GenerateVertices(size);
            var graph = this.CreateGraph(vertices);
            Assert.Equal(size, graph.Size);
            graph.Connect(vertices[0], vertices[1]);
            graph.Connect(vertices[0], vertices[2]);
            graph.Connect(vertices[0], vertices[3]);
            graph.Connect(vertices[1], vertices[3]);
            graph.Connect(vertices[2], vertices[3]);
            graph.Connect(vertices[3], vertices[4]);

            var path = graph.DepthFirstSearchPreOrder(vertices[0]);
            Assert.NotEmpty(path);
            for (var i = size - 1; i >= 0; --i)
            {
                Assert.Contains(vertices[i], path);
            }
        }

        [Fact]
        public void Graph_DepthFirstSearchPostOrder_Succeeds()
        {
            var size = 5;
            var vertices = GenerateVertices(size);
            var graph = this.CreateGraph(vertices);
            Assert.Equal(size, graph.Size);

            graph.Connect(vertices[0], vertices[1]);
            graph.Connect(vertices[0], vertices[2]);
            graph.Connect(vertices[0], vertices[3]);
            graph.Connect(vertices[1], vertices[3]);
            graph.Connect(vertices[2], vertices[3]);
            graph.Connect(vertices[3], vertices[4]);

            var path = graph.DepthFirstSearchPostOrder(vertices[0]);
            Assert.NotEmpty(path);
            for (var i = size - 1; i >= 0; --i)
            {
                Assert.Contains(vertices[i], path);
            }
        }

        [Fact]
        public void Graph_Degrees_Succeeds()
        {
            var size = 3;
            var vertices = GenerateVertices(size);
            var graph = this.CreateGraph(vertices);
            Assert.Equal(size, graph.Size);

            graph.Connect(vertices[0], vertices[1]);
            graph.Connect(vertices[1], vertices[2]);

            if (graph.Type == GraphType.Undirected)
            {
                Assert.Equal(1, graph.Degree(vertices[0]));
                Assert.Equal(2, graph.Degree(vertices[1]));
                Assert.Equal(1, graph.Degree(vertices[2]));
            }
            else
            {
                Assert.Equal(1, graph.Degree(vertices[0]));
                Assert.Equal(1, graph.Degree(vertices[1]));
                Assert.Equal(0, graph.Degree(vertices[2]));
            }
        }

        [Fact]
        public void Graph_Neighbors_Succeeds()
        {
            var size = 3;
            var vertices = GenerateVertices(size);
            var graph = this.CreateGraph(vertices);
            Assert.Equal(size, graph.Size);

            graph.Connect(vertices[0], vertices[1]);
            graph.Connect(vertices[1], vertices[2]);

            var neighbors = graph.Neighbors(vertices[1]);
            if (graph.Type == GraphType.Undirected)
            {
                Assert.Contains(vertices[0], neighbors);
                Assert.Contains(vertices[2], neighbors);
            }
            else
            {
                Assert.DoesNotContain(vertices[0], neighbors);
                Assert.Contains(vertices[2], neighbors);
            }
        }

        [Fact]
        public void Graph_Clone_Succeeds()
        {
            var size = 3;
            var vertices = GenerateVertices(size);
            var graph = this.CreateGraph(vertices);
            Assert.Equal(size, graph.Size);
            graph.Connect(vertices[0], vertices[1]);
            graph.Connect(vertices[1], vertices[2]);

            if (graph.Type == GraphType.Undirected)
            {
                Assert.False(graph.Adjacent(vertices[0], vertices[0]));
                Assert.True(graph.Adjacent(vertices[0], vertices[1]));
                Assert.False(graph.Adjacent(vertices[0], vertices[2]));

                Assert.True(graph.Adjacent(vertices[1], vertices[0]));
                Assert.False(graph.Adjacent(vertices[1], vertices[1]));
                Assert.True(graph.Adjacent(vertices[1], vertices[2]));

                Assert.False(graph.Adjacent(vertices[2], vertices[0]));
                Assert.True(graph.Adjacent(vertices[2], vertices[1]));
                Assert.False(graph.Adjacent(vertices[2], vertices[2]));
            }
            else
            {
                Assert.False(graph.Adjacent(vertices[0], vertices[0]));
                Assert.True(graph.Adjacent(vertices[0], vertices[1]));
                Assert.False(graph.Adjacent(vertices[0], vertices[2]));

                Assert.False(graph.Adjacent(vertices[1], vertices[0]));
                Assert.False(graph.Adjacent(vertices[1], vertices[1]));
                Assert.True(graph.Adjacent(vertices[1], vertices[2]));

                Assert.False(graph.Adjacent(vertices[2], vertices[0]));
                Assert.False(graph.Adjacent(vertices[2], vertices[1]));
                Assert.False(graph.Adjacent(vertices[2], vertices[2]));
            }

            var clone = graph.Clone();

            if (graph.Type == GraphType.Undirected)
            {
                Assert.False(clone.Adjacent(vertices[0], vertices[0]));
                Assert.True(clone.Adjacent(vertices[0], vertices[1]));
                Assert.False(clone.Adjacent(vertices[0], vertices[2]));

                Assert.True(clone.Adjacent(vertices[1], vertices[0]));
                Assert.False(clone.Adjacent(vertices[1], vertices[1]));
                Assert.True(clone.Adjacent(vertices[1], vertices[2]));

                Assert.False(clone.Adjacent(vertices[2], vertices[0]));
                Assert.True(clone.Adjacent(vertices[2], vertices[1]));
                Assert.False(clone.Adjacent(vertices[2], vertices[2]));
            }
            else
            {
                Assert.False(clone.Adjacent(vertices[0], vertices[0]));
                Assert.True(clone.Adjacent(vertices[0], vertices[1]));
                Assert.False(clone.Adjacent(vertices[0], vertices[2]));

                Assert.False(clone.Adjacent(vertices[1], vertices[0]));
                Assert.False(clone.Adjacent(vertices[1], vertices[1]));
                Assert.True(clone.Adjacent(vertices[1], vertices[2]));

                Assert.False(clone.Adjacent(vertices[2], vertices[0]));
                Assert.False(clone.Adjacent(vertices[2], vertices[1]));
                Assert.False(clone.Adjacent(vertices[2], vertices[2]));
            }
        }
    }
}
