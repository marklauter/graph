using Graph.Indexes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
            this.adjacencyIndex = UndirectedAdjacencyList<Guid>.Empty();
        }

        public Graph(IAdjacencyIndex<Guid> adjacencyIndex)
            : base()
        {
            this.edges = new();
            this.nodes = new();
            this.adjacencyIndex = adjacencyIndex ?? throw new ArgumentNullException(nameof(adjacencyIndex));
            this.IsDirected = adjacencyIndex.Type == IndexType.Directed;
        }

        public Graph(Guid id, bool isDirected)
            : base(id)
        {
            this.edges = new();
            this.nodes = new();
            this.IsDirected = isDirected;

            this.adjacencyIndex = this.IsDirected
                ? DirectedAdjacencyList<Guid>.Empty()
                : UndirectedAdjacencyList<Guid>.Empty();
        }

        private Graph(Graph other)
            : base(other)
        {
            this.edges = other.edges
                .Select(e => e.Clone() as Edge)
                .ToHashSet();

            this.nodes = other.nodes.Values
                .Select(n => n.Clone() as Node)
                .ToDictionary(clone => clone.Id, clone => clone);

            this.IsDirected = other.IsDirected;
            this.adjacencyIndex = other.adjacencyIndex.Clone() as IAdjacencyIndex<Guid>;
        }

        private readonly IAdjacencyIndex<Guid> adjacencyIndex;

        [JsonProperty("edges")]
        private readonly HashSet<Edge> edges;

        [JsonIgnore]
        public ImmutableHashSet<Edge> Edges => this.edges.ToImmutableHashSet();

        [JsonProperty("nodes")]
        private readonly Dictionary<Guid, Node> nodes;

        [JsonIgnore]
        public ImmutableHashSet<Node> Nodes => this.nodes.Values.ToImmutableHashSet();

        [JsonProperty("directed")]
        public bool IsDirected { get; }

        public Node Add()
        {
            var node = new Node();
            _ = this.Add(node);
            return node;
        }

        public bool Add(Node node)
        {
            return this.nodes.TryAdd(node.Id, node);
        }

        public int AddRange(IEnumerable<Node> nodes)
        {
            return nodes.Count(n => this.nodes.TryAdd(n.Id, n));
        }

        public override object Clone()
        {
            return new Graph(this);
        }

        public Edge Couple(Guid sourceId, Guid targetId)
        {
            if (!this.nodes.ContainsKey(sourceId))
            {
                throw new KeyNotFoundException(nameof(sourceId));
            }

            if (!this.nodes.ContainsKey(targetId))
            {
                throw new KeyNotFoundException(nameof(targetId));
            }

            _ = this.adjacencyIndex.Couple(sourceId, targetId);

            var edge = new Edge(sourceId, targetId, this.IsDirected);
            return this.edges.Add(edge)
                ? edge
                : null;
        }

        public Edge Couple(Node source, Node target)
        {
            return this.Couple(source.Id, target.Id);
        }

        public bool Couple(Edge edge)
        {
            if (edge is null)
            {
                throw new ArgumentNullException(nameof(edge));
            }

            if (!this.nodes.ContainsKey(edge.Source))
            {
                throw new KeyNotFoundException(nameof(edge.Source));
            }

            if (!this.nodes.ContainsKey(edge.Target))
            {
                throw new KeyNotFoundException(nameof(edge.Target));
            }

            _ = this.adjacencyIndex.Couple(edge.Source, edge.Target);

            return this.edges.Add(edge);
        }

        public bool TryDecouple(Guid sourceId, Guid targetId, out Edge edge)
        {
            if (!this.nodes.ContainsKey(sourceId))
            {
                throw new KeyNotFoundException(nameof(sourceId));
            }

            if (!this.nodes.ContainsKey(targetId))
            {
                throw new KeyNotFoundException(nameof(targetId));
            }

            _ = this.adjacencyIndex.Decouple(sourceId, targetId);

            edge = new Edge(sourceId, targetId, this.IsDirected);
            return this.edges.Remove(edge);
        }

        public bool TryDecouple(Node source, Node target, out Edge edge)
        {
            return this.TryDecouple(source.Id, target.Id, out edge);
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
            if (this.nodes.Remove(node.Id))
            {
                var incidents = this.edges
                    .Where(e => e.Source == node.Id || e.Target == node.Id);

                foreach (var edge in incidents)
                {
                    _ = this.adjacencyIndex.Decouple(edge.Source, edge.Target);
                    _ = this.edges.Remove(edge);
                }

                return true;
            }

            return false;
        }
    }
}
