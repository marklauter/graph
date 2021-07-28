using Graph.Elements;
using System;

namespace Graph.Sets
{
    public sealed class Graph
        : Element
    {
        private readonly EdgeIndex edgeIndex = EdgeIndex.Empty;

        public static Graph Empty => new();

        public Set<Vertex> Vertices { get; } = Set<Vertex>.Empty;

        public Set<Edge> Edges { get; } = Set<Edge>.Empty;

        public int Degree(Guid vertexId)
        {
            return this.edgeIndex.Degree(vertexId);
        }

        public int Degree(Vertex vertex)
        {
            return this.Degree(vertex.Id);
        }
    }
}
