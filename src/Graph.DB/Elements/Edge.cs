using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Graphs.DB.Elements
{
    [DebuggerDisplay("{SourceId} : {TargetId}")]
    [JsonObject("edge")]
    public sealed class Edge<TId>
        : Element
        , IEquatable<Edge<TId>>
        , IEqualityComparer<Edge<TId>>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        [Required]
        [JsonProperty("directed")]
        public bool IsDirected { get; }

        [Key]
        [Required]
        [JsonProperty("source")]
        public TId SourceId { get; }

        [Key]
        [Required]
        [JsonProperty("target")]
        public TId TargetId { get; }

        private Edge() : base() { }

        private Edge([DisallowNull] Edge<TId> other)
            : base(other)
        {
            this.SourceId = other.SourceId;
            this.TargetId = other.TargetId;
            this.IsDirected = other.IsDirected;
        }

        public Edge([DisallowNull] Node<TId> source, [DisallowNull] Node<TId> target)
            : this(source.Id, target.Id, false)
        {
        }

        public Edge([DisallowNull] Node<TId> source, [DisallowNull] Node<TId> target, bool isDirected)
            : this(source.Id, target.Id, isDirected)
        {
        }

        [JsonConstructor]
        public Edge(TId sourceId, TId targetId, bool isDirected)
            : base()
        {
            this.IsDirected = isDirected;
            this.SourceId = sourceId;
            this.TargetId = targetId;
        }

        [Pure]
        public override object Clone()
        {
            return new Edge<TId>(this);
        }

        [Pure]
        public bool Equals(Edge<TId> other)
        {
            return other != null
                && this.SourceId.Equals(other.SourceId)
                && this.TargetId.Equals(other.TargetId)
                && this.IsDirected == other.IsDirected;
        }

        [Pure]
        public bool Equals(Edge<TId> x, Edge<TId> y)
        {
            return x != null && x.Equals(y) || y == null;
        }

        [Pure]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Edge<TId>);
        }

        [Pure]
        public int GetHashCode([DisallowNull] Edge<TId> obj)
        {
            return obj.GetHashCode();
        }

        [Pure]
        public override int GetHashCode()
        {
            return HashCode.Combine(
                this.SourceId,
                this.TargetId,
                this.IsDirected);
        }
    }
}
