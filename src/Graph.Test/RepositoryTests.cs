using Graph.DB.Elements;
using Graph.DB.IO;
using System;
using System.IO;
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

            var repositoryName = "repositorytests" + Guid.NewGuid().ToString();
            try
            {
                var repository = new JsonRepository<Vertex>(repositoryName);
                var entity = repository.Insert(vertex);

                Assert.Equal(vertex.Id, entity.Id);
                Assert.Equal(0, entity.ETag);

                Assert.True(((Vertex)entity).Is(label));
                Assert.Equal(value.ToString(), ((Vertex)entity).Attributes["value"]);
            }
            finally
            {
                Directory.Delete(repositoryName, true);
            }
        }

        [Fact]
        public void Repository_Read_Vertex_Succeeds()
        {
            var vertex = new Vertex();
            var label = nameof(Vertex).ToLowerInvariant();
            vertex.Classify(label);
            var value = Guid.NewGuid();
            vertex.Qualify("value", value);

            var repositoryName = "repositorytests" + Guid.NewGuid().ToString();
            try
            {
                var repository = new JsonRepository<Vertex>(repositoryName);
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
            finally
            {
                Directory.Delete(repositoryName, true);
            }
        }

        [Fact]
        public void Repository_Insert_Edge_Succeeds()
        {
            var source = new Vertex();
            var target = new Vertex();
            var edge = new Edge(source, target);

            var label = nameof(Vertex).ToLowerInvariant();
            edge.Classify(label);
            var value = Guid.NewGuid();
            edge.Qualify("value", value);

            var repositoryName = "repositorytests" + Guid.NewGuid().ToString();
            try
            {
                var repository = new JsonRepository<Edge>(repositoryName);
                var entity = repository.Insert(edge);

                Assert.Equal(edge.Id, entity.Id);
                Assert.Equal(0, entity.ETag);

                Assert.True(((Edge)entity).Is(label));
                Assert.Equal(value.ToString(), ((Edge)entity).Attributes["value"]);
            }
            finally
            {
                Directory.Delete(repositoryName, true);
            }
        }

        [Fact]
        public void Repository_Read_Edge_Succeeds()
        {
            var source = new Vertex();
            var target = new Vertex();
            var edge = new Edge(source, target);

            var label = nameof(Vertex).ToLowerInvariant();
            edge.Classify(label);
            var value = Guid.NewGuid();
            edge.Qualify("value", value);

            var repositoryName = "repositorytests" + Guid.NewGuid().ToString();
            try
            {
                var repository = new JsonRepository<Edge>(repositoryName);
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

                Assert.Equal(edge.Source, ((Edge)entity).Source);
                Assert.Equal(edge.Target, ((Edge)entity).Target);
            }
            finally
            {
                Directory.Delete(repositoryName, true);
            }
        }
    }
}
