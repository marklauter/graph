using Graphs.Indexes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Graphs.Elements
{
    // todo: create an entirely file based graph using file based adjacency list<guid> and a new file based implementation of IIncidenceList
    // todo: the node list can probably be modeled in file system with the repository with some extension of the repo functionality
    // todo: implement a transaction log

    [DebuggerDisplay("{Id}")]
    [JsonObject("graph")]
    public sealed class Graph
        : Element
        , IGraph
        , IEquatable<Graph>
        , IEqualityComparer<Graph>
    {
#pragma warning disable S1144 // Unused private types or members should be removed
        // default constructor is required by serializer
        private Graph()
            : base()
        {
            this.edges = new IncidenceList();
            this.nodes = new();
        }
#pragma warning restore S1144 // Unused private types or members should be removed

        public Graph(IAdjacencyIndex<Guid> adjacencyIndex)
            : base()
        {
            this.edges = new IncidenceList();
            this.nodes = new();
            this.adjacencyIndex = adjacencyIndex ?? throw new ArgumentNullException(nameof(adjacencyIndex));
        }

        public Graph(Guid id, bool isDirected)
            : base(id)
        {
            this.edges = new IncidenceList();
            this.nodes = new();
            this.adjacencyIndex = isDirected
                ? DirectedAdjacencyList<Guid>.Empty()
                : UndirectedAdjacencyList<Guid>.Empty();
        }

        private Graph(Graph other)
            : base(other)
        {
            this.edges = other.edges.Clone() as IIncidenceIndex;

            this.nodes = other.nodes.Values
                .Select(n => n.Clone() as Node)
                .ToDictionary(clone => clone.Id, clone => clone);

            this.IsDirected = other.IsDirected;
            this.adjacencyIndex = other.adjacencyIndex.Clone() as IAdjacencyIndex<Guid>;
        }

        private IAdjacencyIndex<Guid> adjacencyIndex;

        [JsonProperty("matrix")]
#pragma warning disable IDE0051 // Remove unused private members
        // it's required for serialization
        private string Matrix
#pragma warning restore IDE0051 // Remove unused private members
        {
            get => this.adjacencyIndex.ToString();
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    var type = AdjacencyIndex.ParseType(value);
                    this.adjacencyIndex = type == IndexType.Directed
                        ? DirectedAdjacencyList<Guid>.Empty()
                        : UndirectedAdjacencyList<Guid>.Empty();

                    this.adjacencyIndex.Parse(value);
                }
            }
        }

        [JsonProperty("edges")]
        private readonly IIncidenceIndex edges;

        [JsonIgnore]
        public IEnumerable<Edge> Edges => this.edges;

        [JsonProperty("nodes")]
        internal readonly Dictionary<Guid, Node> nodes;

        [JsonIgnore]
        public IEnumerable<Node> Nodes => this.nodes.Values;

        [JsonProperty("directed")]
        public bool IsDirected
        {
            get => this.adjacencyIndex.Type == IndexType.Directed;
            private set => _ = value;  // makes serialization possible
        }

        public int Size { get; }

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

        public bool Adjacent(Guid source, Guid target)
        {
            return this.adjacencyIndex.Adjacent(source, target);
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

        public int Degree(Guid node)
        {
            return this.adjacencyIndex.Degree(node);
        }

        public Edge Edge(Guid id)
        {
            var edge = this.edges
                .FirstOrDefault(edge => edge.Id == id);
            return edge != null
                ? edge
                : throw new KeyNotFoundException(id.ToString());
        }

        public IEnumerable<(Edge edge, NodeTypes nodeType)> IncidentEdges(Node node)
        {
            return this.edges.Edges(node);
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

        public IEnumerable<Node> Neighbors(Node node)
        {
            foreach (var id in this.adjacencyIndex.Neighbors(node.Id))
            {
                yield return this.nodes[id];
            }
        }

        public IEnumerable<Guid> Neighbors(Guid node)
        {
            return this.adjacencyIndex.Neighbors(node);
        }
        
        public Node Node(Guid id)
        {
            return this.nodes.TryGetValue(id, out var node) 
                ? node 
                : throw new KeyNotFoundException(id.ToString());
        }

        public bool Remove(Node node)
        {
            if (this.nodes.Remove(node.Id))
            {
                var incidentEdges = this.edges
                    .Where(e => e.Source == node.Id || e.Target == node.Id);

                foreach (var edge in incidentEdges)
                {
                    _ = this.adjacencyIndex.Decouple(edge.Source, edge.Target);
                    _ = this.edges.Remove(edge);
                }

                return true;
            }

            return false;
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
    }
}
