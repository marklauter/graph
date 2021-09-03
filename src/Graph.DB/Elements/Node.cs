using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Graphs.DB.Elements
{
    [DebuggerDisplay("{Key}")]
    [JsonObject("node")]
    public sealed class Node<TId>
        : Element<TId>
        , IEquatable<Node<TId>>
        , IEqualityComparer<Node<TId>>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        [JsonProperty]
        private readonly ConcurrentHashSet<TId> neighbors = new();

        [JsonProperty]
        private readonly ConcurrentHashSet<TId> edges = new();

        private Node() : base() { }

        private Node([DisallowNull] Node<TId> other)
            : base(other)
        {
            this.neighbors = new ConcurrentHashSet<TId>(other.neighbors);
        }

        [JsonConstructor]
        public Node(TId id)
            : base(id)
        {
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

        public bool Couple([DisallowNull] Node<TId> target)
        {
            // todo: trying to work out the edge ID problem - need a way to generate ID values for TId.. hmmm
            //var edge = new Edge()
            return this.neighbors.Add(target.Id);
        }

        public bool TryDecouple([DisallowNull] Node<TId> target)
        {
            return this.neighbors.Remove(target.Id);
        }

        [Pure]
        public int Degree()
        {
            return this.neighbors.Count;
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
            foreach (var neighbor in this.neighbors)
            {
                yield return neighbor;
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
