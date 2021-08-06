using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Graph.Elements
{
    [DebuggerDisplay("{Id}")]
    [JsonObject("graph")]
    public sealed class Graph
        : Element
        , IGraph
        , IEquatable<Graph>
        , IEqualityComparer<Graph>
    {
        public Graph()
            : base()
        {
            this.edges = new();
            this.nodes = new();
        }

        private Graph(Graph other)
            : base(other)
        {
            this.edges = other.edges
                .Select(e => e.Clone() as Edge)
                .ToHashSet();

            this.nodes = other.nodes
                .Select(n => n.Clone() as Node)
                .ToHashSet();
        }

        [JsonProperty("edges")]
        private readonly HashSet<Edge> edges;

        [JsonProperty("nodes")]
        private readonly HashSet<Node> nodes;

        [JsonProperty("directed")]
        public bool IsDirected { get; }

        public bool Add(Node node)
        {
            return this.nodes.Add(node);
        }

        public int AddRange(IEnumerable<Node> nodes)
        {
            var added = 0;
            foreach (var node in nodes)
            {
                added += this.nodes.Add(node)
                    ? 1
                    : 0;
            }

            return added;
        }

        public override object Clone()
        {
            return new Graph(this);
        }

        public Edge Couple(Guid sourceId, Guid targetId)
        {
            var edge = new Edge(sourceId, targetId);
            return this.edges.Add(edge)
                ? edge
                : null;
        }

        public Edge Couple(Node source, Node target)
        {
            var edge = new Edge(source, target);
            return this.edges.Add(edge)
                ? edge
                : null;
        }

        public bool TryDecouple(Guid sourceId, Guid targetId, out Edge edge)
        {
            edge = new Edge(sourceId, targetId);
            return this.edges.Remove(edge);
        }

        public bool TryDecouple(Node source, Node target, out Edge edge)
        {
            edge = new Edge(source, target);
            return this.edges.Remove(edge);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Graph);
        }

        public bool Equals(Graph other)
        {
            return other != null
                && other.Id == this.Id;
        }

        public bool Equals(Graph x, Graph y)
        {
            return x != null && x.Equals(y) || y == null;
        }

        public int GetHashCode([DisallowNull] Graph obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id);
        }

        public bool Remove(Node node)
        {
            if (this.nodes.Remove(node))
            {
                var incidents = this.edges
                    .Where(e => e.Source == node.Id || e.Target == node.Id);
                foreach (var edge in incidents)
                {
                    _ = this.edges.Remove(edge);
                }

                return true;
            }

            return false;
        }
    }
}
