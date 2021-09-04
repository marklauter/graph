using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Graphs.DB.Elements
{
    [DebuggerDisplay("{Key}")]
    [JsonObject("node")]
    public sealed class Node
        : Element
        , IEquatable<Node>
        , IEqualityComparer<Node>
    {
        [JsonProperty]
        private readonly ConcurrentHashSet<Guid> neighbors = new();

        private readonly ConcurrentDictionary<string, ConcurrentHashSet<Guid>> index = new();

        [JsonProperty]
        private readonly ConcurrentHashSet<Guid> edges = new();

        private Node() : base() { }

        private Node([DisallowNull] Node other)
            : base(other)
        {
            this.neighbors = new ConcurrentHashSet<Guid>(other.neighbors);
        }

        [JsonConstructor]
        public Node(Guid id)
            : base(id)
        {
        }

        [Pure]
        public bool IsAdjacent(Guid targetId)
        {
            return this.neighbors.Contains(targetId);
        }

        [Pure]
        public bool IsAdjacent(Node target)
        {
            return this.IsAdjacent(target.Id);
        }

        [Pure]
        public override object Clone()
        {
            return new Node(this);
        }

        public Edge Couple([DisallowNull] Node target, bool isDirected)
        {
            var edge = new Edge(this, target);
            _ = this.edges.Add(edge.Id);
            _ = this.neighbors.Add(target.Id);
            this.IndexNode(target);

            if (!isDirected)
            {
                _ = target.edges.Add(edge.Id);
                _ = target.neighbors.Add(target.Id);
                target.IndexNode(this);
            }

            return edge;
        }

        public bool TryDecouple([DisallowNull] Node target)
        {
            return this.neighbors.Remove(target.Id);
        }

        [Pure]
        public int Degree()
        {
            return this.neighbors.Count;
        }

        [Pure]
        public bool Equals(Node other)
        {
            return other != null
                && other.Id.Equals(this.Id);
        }

        [Pure]
        public bool Equals(Node x, Node y)
        {
            return x != null && x.Equals(y) || y == null;
        }

        [Pure]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Node);
        }

        [Pure]
        public int GetHashCode([DisallowNull] Node obj)
        {
            return obj.GetHashCode();
        }

        [Pure]
        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id);
        }

        [Pure]
        public IEnumerable<Guid> Neighbors()
        {
            foreach (var neighbor in this.neighbors)
            {
                yield return neighbor;
            }
        }

        private void IndexNode(Node target)
        {
            foreach (var label in target.GetLabels())
            {
                var nodes = this.index.GetOrAdd(label, new ConcurrentHashSet<Guid>());
                nodes.Add(target.Id);
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
