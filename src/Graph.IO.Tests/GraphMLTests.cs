using Graph.IO.GraphML;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Graph.IO.Tests
{
    public sealed class GraphMLTests
    {
        [Fact]
        public void GmlWriter_Write_Read_Succeeds()
        {
            var graph1 = new Elements.Graph();
            var source = graph1.Add();
            var target = graph1.Add();
            graph1.Couple(source, target);
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

                var nodes1 = graph1.Nodes;
                var nodes2 = graph2.Nodes;
                foreach (var node in nodes2)
                {
                    Assert.Contains(node, nodes1);
                }

                var edges1 = graph1.Edges;
                var edges2 = graph2.Edges;

                foreach (var edge in edges2)
                {
                    Assert.Contains(edge, edges1);
                }
            }
            finally
            {
                Directory.Delete(repositoryName, true);
            }
        }
    }
}
