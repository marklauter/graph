using Graphs.Elements;
using System;
using System.IO;
using Xunit;

namespace Graphs.IO.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public void Repository_Insert_Node_Succeeds()
        {
            var node = new Node();
            var label = nameof(Node).ToLowerInvariant();
            node.Classify(label);
            var value = Guid.NewGuid().ToString();
            node.Qualify("value", value);

            var repositoryName = "repositorytests" + Guid.NewGuid().ToString();
            try
            {
                var repository = new JsonRepository<Node>(repositoryName);
                var entity = repository.Insert(node);

                Assert.Equal(node.Id, entity.Id);
                Assert.Equal(0, entity.ETag);

                Assert.True(((Node)entity).Is(label));
                Assert.Equal(value.ToString(), ((Node)entity).Attributes["value"]);
            }
            finally
            {
                Directory.Delete(repositoryName, true);
            }
        }

        [Fact]
        public void Repository_Read_Node_Succeeds()
        {
            var node = new Node();
            var label = nameof(Node).ToLowerInvariant();
            node.Classify(label);
            var value = Guid.NewGuid().ToString();
            node.Qualify("value", value);

            var repositoryName = "repositorytests" + Guid.NewGuid().ToString();
            try
            {
                var repository = new JsonRepository<Node>(repositoryName);
                var entity = repository.Insert(node);

                Assert.Equal(node.Id, entity.Id);
                Assert.Equal(0, entity.ETag);

                Assert.True(((Node)entity).Is(label));
                Assert.Equal(value.ToString(), ((Node)entity).Attributes["value"]);

                entity = repository.Read(entity.Id);

                Assert.Equal(node.Id, entity.Id);
                Assert.Equal(0, entity.ETag);

                Assert.True(((Node)entity).Is(label));
                Assert.Equal(value.ToString(), ((Node)entity).Attributes["value"]);
            }
            finally
            {
                Directory.Delete(repositoryName, true);
            }
        }

        [Fact]
        public void Repository_Insert_Edge_Succeeds()
        {
            var source = new Node();
            var target = new Node();
            var edge = new Edge(source, target);

            var label = nameof(Node).ToLowerInvariant();
            edge.Classify(label);
            var value = Guid.NewGuid().ToString();
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
            var source = new Node();
            var target = new Node();
            var edge = new Edge(source, target);

            var label = nameof(Node).ToLowerInvariant();
            edge.Classify(label);
            var value = Guid.NewGuid().ToString();
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

        [Fact]
        public void Repository_Write_Read_Graph_Succeeds()
        {
            //var graph = new Graph.Elements.Graph();
            //var label = nameof(Node).ToLowerInvariant();
            //node.Classify(label);
            //var value = Guid.NewGuid().ToString();
            //node.Qualify("value", value);

            //var repositoryName = "repositorytests" + Guid.NewGuid().ToString();
            //try
            //{
            //    var repository = new JsonRepository<Node>(repositoryName);
            //    var entity = repository.Insert(node);

            //    Assert.Equal(node.Id, entity.Id);
            //    Assert.Equal(0, entity.ETag);

            //    Assert.True(((Node)entity).Is(label));
            //    Assert.Equal(value.ToString(), ((Node)entity).Attributes["value"]);

            //    entity = repository.Read(entity.Id);

            //    Assert.Equal(node.Id, entity.Id);
            //    Assert.Equal(0, entity.ETag);

            //    Assert.True(((Node)entity).Is(label));
            //    Assert.Equal(value.ToString(), ((Node)entity).Attributes["value"]);
            //}
            //finally
            //{
            //    Directory.Delete(repositoryName, true);
            //}
        }
    }
}
