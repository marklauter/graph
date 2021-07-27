using graph.elements;
using System;
using System.Collections.Generic;

namespace graph.sets
{
    public class Graph
        : Element
    {
        private Graph()
        {
            this.BuildVerticesMap();
        }

        public Graph(string label)
            : base(label)
        {
            this.Nodes = new Set<Node>($"{label} Nodes");
            this.Edges = new Set<Edge>($"{label} Edges");

            this.BuildVerticesMap();
        }

        public Graph(
            string label,
            IEnumerable<Node> nodes,
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

            this.Nodes = new Set<Node>($"{label} Nodes", nodes);
            this.Edges = new Set<Edge>($"{label} Edges", edges);

            this.BuildVerticesMap();
        }

        private readonly Dictionary<Guid, List<Guid>> vertices = new Dictionary<Guid, List<Guid>>();

        public Set<Node> Nodes { get; }

        public Set<Edge> Edges { get; }

        private void BuildVerticesMap()
        {
            foreach (var edge in this.Edges)
            {
                if (!this.vertices.TryGetValue(edge.V1.Id, out var v1Guids))
                {
                    v1Guids = new List<Guid>();
                    this.vertices.Add(edge.V1.Id, v1Guids);
                }

                v1Guids.Add(edge.V2.Id);

                if (!this.vertices.TryGetValue(edge.V2.Id, out var v2Guids))
                {
                    v2Guids = new List<Guid>();
                    this.vertices.Add(edge.V2.Id, v2Guids);
                }

                v2Guids.Add(edge.V1.Id);
            }
        }
    }
}
