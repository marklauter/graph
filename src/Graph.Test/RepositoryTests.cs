using Graph.DB.Elements;
using Graph.DB.IO;
using System;
using Xunit;

namespace Graph.Test
{
    public class RepositoryTests
    {
        [Fact]
        public void Repository_Insert_Vertex_Succeeds()
        {
            var vertex = new Vertex();
            var label = nameof(Vertex).ToLowerInvariant();
            vertex.Classify(label);
            var value = Guid.NewGuid();
            vertex.Qualify("value", value);

            var repository = new JsonRepository<Vertex>("repositorytests");
            var entity = repository.Insert(vertex);

            Assert.Equal(vertex.Id, entity.Id);
            Assert.Equal(0, entity.ETag);

            Assert.True(((Vertex)entity).Is(label));
            Assert.Equal(value.ToString(), ((Vertex)entity).Attributes["value"]);
        }

        [Fact]
        public void Repository_Read_Vertex_Succeeds()
        {
            var vertex = new Vertex();
            var label = nameof(Vertex).ToLowerInvariant();
            vertex.Classify(label);
            var value = Guid.NewGuid();
            vertex.Qualify("value", value);

            var repository = new JsonRepository<Vertex>("repositorytests");
            var entity = repository.Insert(vertex);

            Assert.Equal(vertex.Id, entity.Id);
            Assert.Equal(0, entity.ETag);

            Assert.True(((Vertex)entity).Is(label));
            Assert.Equal(value.ToString(), ((Vertex)entity).Attributes["value"]);

            entity = repository.Read(entity.Id);

            Assert.Equal(vertex.Id, entity.Id);
            Assert.Equal(0, entity.ETag);

            Assert.True(((Vertex)entity).Is(label));
            Assert.Equal(value.ToString(), ((Vertex)entity).Attributes["value"]);
        }

        [Fact]
        public void Repository_Insert_Edge_Succeeds()
        {
            var vertex1 = new Vertex();
            var vertex2 = new Vertex();
            var edge = new Edge(vertex1, vertex2);
            
            var label = nameof(Vertex).ToLowerInvariant();
            edge.Classify(label);
            var value = Guid.NewGuid();
            edge.Qualify("value", value);

            var repository = new JsonRepository<Edge>("repositorytests");
            var entity = repository.Insert(edge);

            Assert.Equal(edge.Id, entity.Id);
            Assert.Equal(0, entity.ETag);

            Assert.True(((Edge)entity).Is(label));
            Assert.Equal(value.ToString(), ((Edge)entity).Attributes["value"]);
        }

        [Fact]
        public void Repository_Read_Edge_Succeeds()
        {
            var vertex1 = new Vertex();
            var vertex2 = new Vertex();
            var edge = new Edge(vertex1, vertex2);

            var label = nameof(Vertex).ToLowerInvariant();
            edge.Classify(label);
            var value = Guid.NewGuid();
            edge.Qualify("value", value);

            var repository = new JsonRepository<Edge>("repositorytests");
            var entity = repository.Insert(edge);

            Assert.Equal(edge.Id, entity.Id);
            Assert.Equal(0, entity.ETag);

            Assert.True(((Edge)entity).Is(label));
            Assert.Equal(value.ToString(), ((Edge)entity).Attributes["value"]);

            entity = repository.Read(entity.Id);

            Assert.Equal(edge.Id, entity.Id);
            Assert.Equal(0, entity.ETag);

            Assert.True(((Edge)entity).Is(label));
            Assert.Equal(value.ToString(), ((Edge)entity).Attributes["value"]);

            Assert.Equal(edge.Vertex1, ((Edge)entity).Vertex1);
            Assert.Equal(edge.Vertex2, ((Edge)entity).Vertex2);
        }
    }
}
