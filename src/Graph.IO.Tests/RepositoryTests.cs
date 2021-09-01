using Graphs.Elements;
using Graphs.Indexes;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Xunit;

namespace Graphs.IO.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public void FileAccessTest()
        {
            if(File.Exists("test.txt"))
            File.Delete("test.txt");

            var stream1 = new FileStream("test.txt", FileMode.CreateNew, FileAccess.Write, FileShare.None);
            Assert.NotNull(stream1);
            FileStream stream2 = null;
            var open = false;
            var tries = 0;
            while (!open)
            {
                try
                {
                    stream2 = new FileStream("test.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
                    open = true;
                }
                catch(Exception ex)
                {
                    if (!FileIsLocked(ex as IOException))
                    {
                        throw;
                    }
                    else
                    {
                        ++tries;
                        if (tries > 10)
                        {
                            stream1.Close();
                            stream1.Dispose();
                        }
                    }
                }
            }
            Assert.NotNull(stream2);
            stream2.Close();
            stream2.Dispose();

            File.Delete("test.txt");
        }

        private const uint HRFileLocked = 0x80070020;
        private const uint HRPortionOfFileLocked = 0x80070021;

        private static bool FileIsLocked(IOException ioException)
        {
            var errorCode = (uint)Marshal.GetHRForException(ioException);
            return errorCode == HRFileLocked || errorCode == HRPortionOfFileLocked;
        }

        [Fact]
        public void Repository_Insert_Node_Succeeds()
        {
            var index = UndirectedAdjacencyList<Guid>.Empty();
            var graph = new Graph(index);

            var node = graph.Add();
            var label = nameof(Node).ToLowerInvariant();
            node.Classify(label);
            var value = Guid.NewGuid().ToString();
            node.Qualify("value", value);

            var repositoryName = "repositorytests.node." + Guid.NewGuid().ToString();
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
            var index = UndirectedAdjacencyList<Guid>.Empty();
            var graph = new Graph(index);

            var node = graph.Add();
            var label = nameof(Node).ToLowerInvariant();
            node.Classify(label);
            var value = Guid.NewGuid().ToString();
            node.Qualify("value", value);

            var repositoryName = "repositorytests.node." + Guid.NewGuid().ToString();
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
            var index = UndirectedAdjacencyList<Guid>.Empty();
            var graph = new Graph(index);

            var source = graph.Add();
            var target = graph.Add();

            var label = nameof(Node).ToLowerInvariant();
            var value = Guid.NewGuid().ToString();

            var edge = (Edge)new Edge(source, target)
                .Classify(label)
                .Qualify("value", value);

            var repositoryName = "repositorytests.edge." + Guid.NewGuid().ToString();
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
            var index = UndirectedAdjacencyList<Guid>.Empty();
            var graph = new Graph(index);

            var source = graph.Add();
            var target = graph.Add();

            var label = nameof(Node).ToLowerInvariant();
            var value = Guid.NewGuid().ToString();

            var edge = (Edge)new Edge(source, target)
                .Classify(label)
                .Qualify("value", value);

            var repositoryName = "repositorytests.edge." + Guid.NewGuid().ToString();
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
            var graphLabel = "adventure";
            var version = "0.0";
            var index = UndirectedAdjacencyList<Guid>.Empty();
            var graph = new Graph(index);

            graph.Classify(graphLabel)
                .Qualify("version", version);

            var actionGo = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "go");

            var actionLook = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "look");

            var actionUse = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "use");

            var actionRead = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "read");

            var actionTake = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "take");

            var actionDrop = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "drop");

            var actionOpen = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "open");

            var actionClose = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "close");

            var cell1 = (Node)graph.Add()
                .Classify("cell")
                .Qualify("size", "10")
                .Qualify("name", "spawn point")
                .Qualify("description", "empty field")
                .Qualify("actions", "look go");
            graph.Couple(cell1, actionLook).Classify("action");
            graph.Couple(cell1, actionGo).Classify("action");

            var cell2 = (Node)graph.Add()
                .Classify("cell")
                .Qualify("size", "20")
                .Qualify("name", "castle")
                .Qualify("description", "crumbling castle");
            graph.Couple(cell2, actionLook).Classify("action");
            graph.Couple(cell2, actionGo).Classify("action");

            graph.Couple(cell1, cell2)
                .Classify("road")
                .Qualify("length", "10");

            var sword = (Node)graph.Add()
                .Classify("object")
                .Qualify("name", "rusty sword")
                .Qualify("description", "a sword with rust on it");
            graph.Couple(sword, actionLook).Classify("action");
            graph.Couple(sword, actionTake).Classify("action");
            graph.Couple(sword, actionUse).Classify("action");
            graph.Couple(sword, actionDrop).Classify("action");

            var chest = (Node)graph.Add()
                .Classify("object")
                .Qualify("name", "wooden chest")
                .Qualify("description", "a chest made of wood");
            graph.Couple(chest, actionLook).Classify("action");
            graph.Couple(chest, actionOpen).Classify("action");
            graph.Couple(chest, actionClose).Classify("action");

            var scroll = (Node)graph.Add()
                .Classify("object")
                .Qualify("name", "magic scroll")
                .Qualify("description", "an ancient scroll containing magic spell");
            graph.Couple(scroll, actionLook).Classify("action");
            graph.Couple(scroll, actionTake).Classify("action");
            graph.Couple(scroll, actionRead).Classify("action");
            graph.Couple(scroll, actionDrop).Classify("action");

            graph.Couple(cell1, sword)
                .Classify("contains");

            graph.Couple(cell2, chest)
                .Classify("contains");

            graph.Couple(chest, scroll)
                .Classify("contains");

            var repositoryName = "repositorytests.graph." + Guid.NewGuid().ToString();
            try
            {
                var repository = new JsonRepository<Graph>(repositoryName);
                var entity = repository.Insert(graph);

                Assert.Equal(graph.Id, entity.Id);
                Assert.Equal(0, entity.ETag);

                Assert.True(((Graph)entity).Is(graphLabel));
                Assert.Equal(version, ((Graph)entity).Attributes["version"]);

                entity = repository.Read(entity.Id);

                Assert.Equal(graph.Id, entity.Id);
                Assert.Equal(0, entity.ETag);

                Assert.True(((Graph)entity).Is(graphLabel));
                Assert.Equal(version, ((Graph)entity).Attributes["version"]);

                var clone = (Graph)entity;
                var cloneNodes = clone.Nodes;
                var cloneEdges = clone.Edges;

                Assert.NotEmpty(cloneNodes);
                Assert.NotEmpty(cloneEdges);

                foreach (var node in graph.Nodes)
                {
                    Assert.Contains(node, cloneNodes);
                }

                foreach (var edge in graph.Edges)
                {
                    Assert.Contains(edge, cloneEdges);
                }
            }
            finally
            {
                Directory.Delete(repositoryName, true);
            }
        }
    }
}
