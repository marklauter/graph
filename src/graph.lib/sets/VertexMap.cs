using graph.elements;
using System;
using System.Collections.Generic;

namespace graph.sets
{
    internal sealed class VertexMap
    {
        public VertexMap(Set<Edge> edges)
        {
            this.map = new Dictionary<Guid, List<Guid>>();

            foreach (var edge in edges)
            {
                if (!this.map.TryGetValue(edge.Vertex1.Id, out var v1Guids))
                {
                    v1Guids = new List<Guid>();
                    this.map.Add(edge.Vertex1.Id, v1Guids);
                }

                v1Guids.Add(edge.Vertex2.Id);

                if (!this.map.TryGetValue(edge.Vertex2.Id, out var v2Guids))
                {
                    v2Guids = new List<Guid>();
                    this.map.Add(edge.Vertex2.Id, v2Guids);
                }

                v2Guids.Add(edge.Vertex1.Id);
            }
        }

        private readonly Dictionary<Guid, List<Guid>> map;

        public List<Guid> this[Guid g] => this.map[g];
    }
}
