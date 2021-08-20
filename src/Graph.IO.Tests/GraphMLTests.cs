using Graphs.IO.GraphML;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Graphs.IO.Tests
{
    public sealed class GraphMLTests
    {
        [Fact]
        public void GmlWriter_Write_Read_Succeeds()
        {
            var graph1 = new Elements.Graph();
            graph1.Classify("testgraphclass");
            graph1.Qualify("graphAttribute", "testgraphvalue");

            var source1 = graph1.Add();
            source1.Classify("sourceclass");
            source1.Qualify("nodeAttribute", "sourcevalue");

            var target1 = graph1.Add();
            target1.Classify("targetclass");
            target1.Qualify("nodeAttribute", "targetvalue");

            var edge1 = graph1.Couple(source1, target1);
            edge1.Classify("edgeclass");
            edge1.Qualify("edgeAttribute", "edgevalue");

            Assert.Equal(2, graph1.Nodes.Count);
            Assert.Single(graph1.Edges);

            var repositoryName = $"gmlwriter_tests_folder_{Guid.NewGuid()}";
            var fileName = Path.Combine(repositoryName, $"gmlwriter_tests_file_{Guid.NewGuid()}.xml");
            try
            {
                if (!Directory.Exists(repositoryName))
                {
                    Directory.CreateDirectory(repositoryName);
                }

                using var writeStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
                GmlWriter.Write(graph1, writeStream);
                writeStream.Close();

                using var readStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                var graph2 = GmlReader.Read(readStream).Single();
                readStream.Close();

                Assert.Equal(graph1.Id, graph2.Id);

                var nodes1 = graph1.Nodes;
                var nodes2 = graph2.Nodes;

                Assert.Equal(nodes1.Count, nodes2.Count);

                foreach (var node1 in nodes1)
                {
                    Assert.Contains(node1, nodes2);
                }

                foreach (var node2 in nodes2)
                {
                    Assert.Contains(node2, nodes1);
                }

                foreach (var node1 in nodes1)
                {
                    var node2 = nodes2.Single(n => n.Equals(node1));
                    foreach (var @class in node1.Labels)
                    {
                        Assert.True(node2.Is(@class));
                    }

                    foreach (var @class in node2.Labels)
                    {
                        Assert.True(node1.Is(@class));
                    }
                }

                foreach (var node1 in nodes1)
                {
                    var node2 = nodes2.Single(n => n.Equals(node1));
                    foreach (var attribute in node1.Attributes.Keys)
                    {
                        Assert.True(node2.HasAttribute(attribute));
                        Assert.Equal(node1.Attributes[attribute], node2.Attributes[attribute]);
                    }

                    foreach (var attribute in node2.Attributes.Keys)
                    {
                        Assert.True(node1.HasAttribute(attribute));
                        Assert.Equal(node2.Attributes[attribute], node1.Attributes[attribute]);
                    }
                }

                var edges1 = graph1.Edges;
                var edges2 = graph2.Edges;

                Assert.Equal(edges1.Count, edges2.Count);

                foreach (var e1 in edges1)
                {
                    Assert.Contains(e1, edges2);
                }

                foreach (var e2 in edges2)
                {
                    Assert.Contains(e2, edges1);
                }

                foreach (var e1 in edges1)
                {
                    var e2 = edges2.Single(e => e.Equals(e1));
                    foreach (var @class in e1.Labels)
                    {
                        Assert.True(e2.Is(@class));
                    }

                    foreach (var @class in e2.Labels)
                    {
                        Assert.True(e1.Is(@class));
                    }
                }

                foreach (var e1 in edges1)
                {
                    var e2 = edges2.Single(e => e.Equals(e1));
                    foreach (var attribute in e1.Attributes.Keys)
                    {
                        Assert.True(e2.HasAttribute(attribute));
                        Assert.Equal(e1.Attributes[attribute], e2.Attributes[attribute]);
                    }

                    foreach (var attribute in e2.Attributes.Keys)
                    {
                        Assert.True(e1.HasAttribute(attribute));
                        Assert.Equal(e2.Attributes[attribute], e1.Attributes[attribute]);
                    }
                }
            }
            finally
            {
                Directory.Delete(repositoryName, true);
            }
        }
    }
}
