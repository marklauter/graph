using Graphs.Elements;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphs.Indexes
{
    [JsonObject("incidenceList")]
    public class IncidenceList
        : IIncidenceIndex
    {
        [JsonProperty("sources")]
        private readonly Dictionary<Guid, HashSet<Edge>> sources = new();

        [JsonProperty("targets")]
        private readonly Dictionary<Guid, HashSet<Edge>> targets = new();

        public IncidenceList() { }

        private IncidenceList(IncidenceList other)
        {
            this.sources = new(other.sources);
            this.targets = new(other.targets);
        }

        public bool Add(Edge edge)
        {
            if (!this.sources.TryGetValue(edge.Source, out var sourceEdges))
            {
                sourceEdges = new HashSet<Edge>();
                this.sources.Add(edge.Source, sourceEdges);
            }

            if (!this.targets.TryGetValue(edge.Target, out var targetEdges))
            {
                targetEdges = new HashSet<Edge>();
                this.targets.Add(edge.Target, targetEdges);
            }

            return sourceEdges.Add(edge)
                && targetEdges.Add(edge);
        }

        public void AddRange(IEnumerable<Edge> edges)
        {
            foreach (var edge in edges)
            {
                _ = this.Add(edge);
            }
        }

        public object Clone()
        {
            return new IncidenceList(this);
        }

        public IEnumerable<(Edge edge, NodeTypes nodeType)> Edges(Node node)
        {
            return this.Edges(node, NodeTypes.Source | NodeTypes.Target);
        }

        public IEnumerable<(Edge edge, NodeTypes nodeType)> Edges(Node node, NodeTypes type)
        {
            if ((type & NodeTypes.Source) == NodeTypes.Source
                && this.sources.TryGetValue(node.Id, out var sourceEdges))
            {
                foreach (var edge in sourceEdges)
                {
                    yield return (edge, NodeTypes.Source);
                }
            }

            if ((type & NodeTypes.Target) == NodeTypes.Target
                && this.targets.TryGetValue(node.Id, out var targetEdges))
            {
                foreach (var edge in targetEdges)
                {
                    yield return (edge, NodeTypes.Target);
                }
            }
        }

        public IEnumerator<Edge> GetEnumerator()
        {
            foreach (var edge in this.sources.Values.SelectMany(set => set).Distinct())
            {
                yield return edge;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool Remove(Node node)
        {
            return this.sources.Remove(node.Id)
                && this.targets.Remove(node.Id);
        }

        public bool Remove(Edge edge)
        {
            return this.sources.TryGetValue(edge.Source, out var sourceEdges)
                && sourceEdges.Remove(edge)
                && this.targets.TryGetValue(edge.Target, out var targetEdges)
                && targetEdges.Remove(edge);
        }
    }
}
