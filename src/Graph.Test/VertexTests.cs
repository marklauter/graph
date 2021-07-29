using Graph.Elements;
using System;
using System.Linq;
using Xunit;

namespace Graph.Test
{
    public class VertexTests
    {
        [Fact]
        public void Vertex_Default_Constructor_Succeeds()
        {
            var vertex = new Vertex();
            Assert.NotNull(vertex);
            Assert.NotEqual(Guid.Empty, vertex.Id);
            Assert.Empty(vertex.Classifications);
            Assert.Empty(vertex.Features);
        }

        [Fact]
        public void Vertex_Classify_Single_Succeeds()
        {
            var vertex = new Vertex();
            Assert.NotNull(vertex);

            var classification = "class";
            vertex.Classify(classification);
            Assert.NotEmpty(vertex.Classifications);
            Assert.Equal(classification, vertex.Classifications[0]);
        }

        [Fact]
        public void Vertex_Classify_Single_Fails_NullOrEmpty_Check()
        {
            var vertex = new Vertex();
            Assert.NotNull(vertex);

            var classification = String.Empty;
            Assert.Throws<ArgumentException>(() => vertex.Classify(classification));
        }

        [Fact]
        public void Vertex_Classify_Enumerable_Succeeds()
        {
            var vertex = new Vertex();
            Assert.NotNull(vertex);

            var classifications = new string[] { "class1", "class2"};
            vertex.Classify(classifications);
            Assert.NotEmpty(vertex.Classifications);
            Assert.Equal(classifications[0], vertex.Classifications[0]);
            Assert.Equal(classifications[1], vertex.Classifications[1]);
        }

        [Fact]
        public void Vertex_Classify_Enumerable_Succeeds_And_Ignores_Duplicate()
        {
            var vertex = new Vertex();
            Assert.NotNull(vertex);

            var classifications = new string[] { "class1", "class1" };
            vertex.Classify(classifications);
            Assert.NotEmpty(vertex.Classifications);
            Assert.Equal(1, vertex.Classifications.Count);
            Assert.Equal(classifications[0], vertex.Classifications[0]);
        }
    }
}
