using Graph.Sets;
using System;
using System.Linq;
using Xunit;

namespace Graph.Test
{
    public class NonDirectedAdjacencyMatrixTests
    {
        [Fact]
        public void AdjacencyMatrix_Empty_Succeeds()
        {
            var matrix = NonDirectedAdjacencyMatrix.Empty;
            Assert.Equal(0, matrix.Size);
        }

        [Fact]
        public void AdjacencyMatrix_Resize_Succeeds()
        {
            var size = 13;
            var matrix = NonDirectedAdjacencyMatrix.Empty.Resize(size);
            Assert.Equal(size, matrix.Size);
        }

        [Fact]
        public void AdjacencyMatrix_Connect_Succeeds()
        {
            var size = 2;
            var matrix = NonDirectedAdjacencyMatrix.Empty.Resize(size);
            Assert.Equal(size, matrix.Size);
            matrix.Connect(0, 1);
            Assert.False(matrix.Adjacent(0, 0));
            Assert.True(matrix.Adjacent(0, 1));
            Assert.True(matrix.Adjacent(1, 0));
            Assert.False(matrix.Adjacent(1, 1));
        }

        [Fact]
        public void AdjacencyMatrix_Resize_Is_NonDestructive()
        {
            var size = 2;
            var matrix = NonDirectedAdjacencyMatrix.Empty.Resize(size);
            Assert.Equal(size, matrix.Size);
            matrix.Connect(0, 1);
            Assert.False(matrix.Adjacent(0, 0));
            Assert.True(matrix.Adjacent(0, 1));
            Assert.True(matrix.Adjacent(1, 0));
            Assert.False(matrix.Adjacent(1, 1));

            size = 3;
            matrix = matrix.Resize(size);
            Assert.Equal(size, matrix.Size);
            Assert.False(matrix.Adjacent(0, 0));
            Assert.True(matrix.Adjacent(0, 1));
            Assert.True(matrix.Adjacent(1, 0));
            Assert.False(matrix.Adjacent(1, 1));
            Assert.False(matrix.Adjacent(0, 2));
            Assert.False(matrix.Adjacent(1, 2));
            Assert.False(matrix.Adjacent(2, 1));
            Assert.False(matrix.Adjacent(2, 0));
        }

        [Fact]
        public void AdjacencyMatrix_DepthFirstSearchPreOrder_Succeeds()
        {
            var size = 5;
            var matrix = NonDirectedAdjacencyMatrix.Empty.Resize(size);
            Assert.Equal(size, matrix.Size);
            matrix.Connect(0, 1);
            matrix.Connect(0, 2);
            matrix.Connect(0, 3);
            matrix.Connect(1, 3);
            matrix.Connect(2, 3);
            matrix.Connect(3, 4);

            Assert.True(matrix.Adjacent(0, 1));
            Assert.True(matrix.Adjacent(0, 2));
            Assert.True(matrix.Adjacent(0, 3));
            Assert.False(matrix.Adjacent(0, 4));

            Assert.True(matrix.Adjacent(1, 0));
            Assert.False(matrix.Adjacent(1, 2));
            Assert.True(matrix.Adjacent(1, 3));
            Assert.False(matrix.Adjacent(1, 4));

            Assert.True(matrix.Adjacent(2, 0));
            Assert.False(matrix.Adjacent(2, 1));
            Assert.True(matrix.Adjacent(2, 3));
            Assert.False(matrix.Adjacent(2, 4));

            Assert.True(matrix.Adjacent(3, 0));
            Assert.True(matrix.Adjacent(3, 1));
            Assert.True(matrix.Adjacent(3, 2));
            Assert.True(matrix.Adjacent(3, 4));

            Assert.False(matrix.Adjacent(4, 0));
            Assert.False(matrix.Adjacent(4, 1));
            Assert.False(matrix.Adjacent(4, 2));
            Assert.True(matrix.Adjacent(4, 3));

            var nodes = matrix.DepthFirstSearchPreOrder(0);
            Assert.NotEmpty(nodes);
            for(var i = size - 1; i >= 0; --i)
            {
                Assert.Contains(i, nodes);
            }
        }

        [Fact]
        public void AdjacencyMatrix_DepthFirstSearchPostOrder_Succeeds()
        {
            var size = 5;
            var matrix = NonDirectedAdjacencyMatrix.Empty.Resize(size);
            Assert.Equal(size, matrix.Size);
            matrix.Connect(0, 1);
            matrix.Connect(0, 2);
            matrix.Connect(0, 3);
            matrix.Connect(1, 3);
            matrix.Connect(2, 3);
            matrix.Connect(3, 4);

            Assert.True(matrix.Adjacent(0, 1));
            Assert.True(matrix.Adjacent(0, 2));
            Assert.True(matrix.Adjacent(0, 3));
            Assert.False(matrix.Adjacent(0, 4));

            Assert.True(matrix.Adjacent(1, 0));
            Assert.False(matrix.Adjacent(1, 2));
            Assert.True(matrix.Adjacent(1, 3));
            Assert.False(matrix.Adjacent(1, 4));

            Assert.True(matrix.Adjacent(2, 0));
            Assert.False(matrix.Adjacent(2, 1));
            Assert.True(matrix.Adjacent(2, 3));
            Assert.False(matrix.Adjacent(2, 4));

            Assert.True(matrix.Adjacent(3, 0));
            Assert.True(matrix.Adjacent(3, 1));
            Assert.True(matrix.Adjacent(3, 2));
            Assert.True(matrix.Adjacent(3, 4));

            Assert.False(matrix.Adjacent(4, 0));
            Assert.False(matrix.Adjacent(4, 1));
            Assert.False(matrix.Adjacent(4, 2));
            Assert.True(matrix.Adjacent(4, 3));

            var nodes = matrix.DepthFirstSearchPostOrder(0);
            Assert.NotEmpty(nodes);
            for (var i = size - 1; i >= 0; --i)
            {
                Assert.Contains(i, nodes);
            }
        }

        [Fact]
        public void AdjacencyMatrix_Degrees_Succeeds()
        {
            var size = 3;
            var matrix = NonDirectedAdjacencyMatrix.Empty.Resize(size);
            Assert.Equal(size, matrix.Size);
            matrix.Connect(0, 1);
            matrix.Connect(1, 2);

            Assert.False(matrix.Adjacent(0, 0));
            Assert.True(matrix.Adjacent(0, 1));
            Assert.False(matrix.Adjacent(0, 2));
            
            Assert.True(matrix.Adjacent(1, 0));
            Assert.False(matrix.Adjacent(1, 1));
            Assert.True(matrix.Adjacent(1, 2));

            Assert.False(matrix.Adjacent(2, 0));
            Assert.True(matrix.Adjacent(2, 1));
            Assert.False(matrix.Adjacent(2, 2));

            Assert.Equal(1, matrix.Degree(0));
            Assert.Equal(2, matrix.Degree(1));
            Assert.Equal(1, matrix.Degree(2));
        }

        [Fact]
        public void AdjacencyMatrix_Neighbors_Succeeds()
        {
            var size = 3;
            var matrix = NonDirectedAdjacencyMatrix.Empty.Resize(size);
            Assert.Equal(size, matrix.Size);
            matrix.Connect(0, 1);
            matrix.Connect(1, 2);

            Assert.False(matrix.Adjacent(0, 0));
            Assert.True(matrix.Adjacent(0, 1));
            Assert.False(matrix.Adjacent(0, 2));

            Assert.True(matrix.Adjacent(1, 0));
            Assert.False(matrix.Adjacent(1, 1));
            Assert.True(matrix.Adjacent(1, 2));

            Assert.False(matrix.Adjacent(2, 0));
            Assert.True(matrix.Adjacent(2, 1));
            Assert.False(matrix.Adjacent(2, 2));

            var neighbors = matrix.Neighbors(1);
            Assert.Contains(0, neighbors);
            Assert.Contains(2, neighbors);
        }
    }
}
