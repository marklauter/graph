using Graph.Elements;
using System;

namespace Graph.Sets
{
    public sealed class Graph
        : Element
    {
        private readonly NonDirectedAdjacencyList edgeIndex = NonDirectedAdjacencyList.Empty;
        private readonly Set<Vertex> vertices = Set<Vertex>.Empty;
        private readonly Set<Edge> edges = Set<Edge>.Empty;

        public static Graph Empty => new();


        public int Degree(Guid vertexId)
        {
            return this.edgeIndex.Degree(vertexId);
        }

        public int Degree(Vertex vertex)
        {
            return this.Degree(vertex.Id);
        }

        public int Size => this.vertices.Size;
    }
}
