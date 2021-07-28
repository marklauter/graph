using graph.elements;
using System;
using System.Collections.Generic;

namespace graph.sets
{
    public class Graph
        : Element
    {
        private Graph() { }

        public Graph(IEnumerable<string> classifications)
            : base(classifications)
        {
            if (classifications is null)
            {
                throw new ArgumentNullException(nameof(classifications));
            }

            this.Vertices = Set<Vertex>.Empty;
            this.Edges = Set<Edge>.Empty;
            this.map = VertexMap.Empty;
        }

        public Graph(
            string label,
            IEnumerable<Vertex> nodes,
            IEnumerable<Edge> edges)
            : base(label)
        {
            if (nodes is null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            if (edges is null)
            {
                throw new ArgumentNullException(nameof(edges));
            }

            this.Vertices = new Set<Vertex>($"{label} Nodes", nodes);
            this.Edges = new Set<Edge>($"{label} Edges", edges);
            this.map = new VertexMap(this.Edges);
        }        

        public Set<Vertex> Vertices { get; }

        public Set<Edge> Edges { get; }

        private readonly VertexMap map;

        public int Degree(Vertex vertex)
        {
            return this.map.Degree(vertex.Id);
        }
    }
}
