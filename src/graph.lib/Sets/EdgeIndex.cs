using Graph.Elements;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Graph.Sets
{
    internal sealed class EdgeIndex
    {
        private readonly ImmutableDictionary<Guid, Set<Edge>> map;
        private readonly Set<Edge> edges;

        public static readonly EdgeIndex Empty = new();

        private EdgeIndex()
        {
            this.map = ImmutableDictionary<Guid, Set<Edge>>.Empty;
        }

        private EdgeIndex(Set<Edge> edges)
        {
            this.edges = edges ?? throw new ArgumentNullException(nameof(edges));

            var leftGroup = edges.Elements
                .GroupBy(edge => edge.Vertex1.Id, e => e)
                .Select(g => new { VertexId = g.Key, Edges = (Set<Edge>)g.ToArray() });

            var rightGroup = edges.Elements
                .GroupBy(edge => edge.Vertex2.Id, e => e)
                .Select(g => new { VertexId = g.Key, Edges = (Set<Edge>)g.ToArray() });

            var incidentalEdges = leftGroup
                .Union(rightGroup);

            this.map = incidentalEdges
                .ToImmutableDictionary(iv => iv.VertexId, iv => iv.Edges);
        }

        public EdgeIndex IndexRange(Set<Edge> edges)
        {
            edges = this.edges != null
                ? this.edges.Union(edges)
                : edges;

            return new EdgeIndex(edges);
        }

        internal Set<Edge> this[Guid key] => this.map[key];

        internal int Degree(Guid key)
        {
            return this.map.TryGetValue(key, out var edges)
                ? edges.Size
                : 0;
        }
    }
}
