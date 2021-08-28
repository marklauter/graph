using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Graphs.DB.Elements
{
    [DebuggerDisplay("{Id}")]
    [JsonObject("node")]
    public sealed class Node<TId>
        : Element
        , IEquatable<Node<TId>>
        , IEqualityComparer<Node<TId>>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        [JsonProperty]
        private readonly HashSet<TId> neighbors = null!;

        [JsonProperty]
        private readonly Dictionary<TId, Edge<TId>> edges = null!;

        /// <summary>
        /// Gets the Id of the element.
        /// </summary>
        [JsonProperty("id")]
        public TId Id { get; }

        private Node() : base() { }

        private Node([DisallowNull] Node<TId> other)
            : base(other)
        {

        }

        [JsonConstructor]
        public Node(TId id)
            : base()
        {
            this.Id = id;
        }

        [Pure]
        public bool Adjacent(TId targetId)
        {
            return this.neighbors.Contains(targetId);
        }

        [Pure]
        public bool Adjacent(Node<TId> target)
        {
            return this.Adjacent(target.Id);
        }

        [Pure]
        public override object Clone()
        {
            return new Node<TId>(this);
        }

        public Edge<TId> Couple([DisallowNull] Node<TId> target)
        {
            return this.Couple(target, false);
        }

        public Edge<TId> Couple([DisallowNull] Node<TId> target, bool isDirected)
        {
            var edge = new Edge<TId>(this, target, isDirected);

            this.neighbors.Add(target.Id);
            this.edges.Add(target.Id, edge);

            if (!isDirected)
            {
                target.neighbors.Add(this.Id);
                target.edges.Add(this.Id, edge);
            }

            return edge;
        }

        public bool TryDecouple([DisallowNull] Node<TId> target, out Edge<TId> edge)
        {
            var result = this.edges.TryGetValue(target.Id, out edge)
                && this.neighbors.Remove(target.Id)
                && this.edges.Remove(target.Id);

            result = result && edge.IsDirected
                || target.neighbors.Remove(this.Id)
                && target.edges.Remove(this.Id);

            return result;
        }

        [Pure]
        public int Degree()
        {
            return this.neighbors.Count;
        }

        public Edge<TId> Edge(Node<TId> target)
        {
            return this.Edge(target.Id);
        }

        public Edge<TId> Edge(TId targetId)
        {
            return this.edges.TryGetValue(targetId, out var edge)
                ? edge
                : throw new KeyNotFoundException(targetId.ToString());
        }

        [Pure]
        public IEnumerable<Edge<TId>> Edges()
        {
            foreach (var edge in this.edges.Values)
            {
                yield return edge;
            }
        }

        [Pure]
        public bool Equals(Node<TId> other)
        {
            return other != null
                && other.Id.Equals(this.Id);
        }

        [Pure]
        public bool Equals(Node<TId> x, Node<TId> y)
        {
            return x != null && x.Equals(y) || y == null;
        }

        [Pure]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Node<TId>);
        }

        [Pure]
        public int GetHashCode([DisallowNull] Node<TId> obj)
        {
            return obj.GetHashCode();
        }

        [Pure]
        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id);
        }

        [Pure]
        public IEnumerable<TId> Neighbors()
        {
            foreach (var id in this.neighbors)
            {
                yield return id;
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
