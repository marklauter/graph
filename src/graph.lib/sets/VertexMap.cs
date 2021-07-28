using graph.elements;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace graph.sets
{
    internal sealed class VertexMap
    {
        public static readonly VertexMap Empty = new VertexMap();

        private VertexMap()
        {
            this.map = ImmutableDictionary<Guid, Guid[]>.Empty;
        }

        public VertexMap(Set<Edge> edges)
        {
            var leftGroup = edges.Elements
                .GroupBy(edge => edge.Vertex1.Id, e => e.Vertex2.Id)
                .Select(g => new { VertexId = g.Key, IncidentVertices = g.ToArray() });

            var rightGroup = edges.Elements
                .GroupBy(edge => edge.Vertex2.Id, e => e.Vertex1.Id)
                .Select(g => new { VertexId = g.Key, IncidentVertices = g.ToArray() });

            var allIncidentalVertices = leftGroup
                .Union(rightGroup);

            this.map = allIncidentalVertices
                .ToImmutableDictionary(iv => iv.VertexId, iv => iv.IncidentVertices);
        }

        private readonly ImmutableDictionary<Guid, Guid[]> map;

        internal Guid[] this[Guid key] => this.map[key];

        internal int Degree(Guid key)
        {
            return this.map.TryGetValue(key, out var guids)
                ? guids.Length
                : 0;
        }
    }
}
