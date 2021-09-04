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
    public sealed class Edge
        : Element
        , IEquatable<Edge>
        , IEqualityComparer<Edge>
    {
        [Required]
        [JsonProperty("directed")]
        public bool IsDirected { get; }

        [Required]
        [JsonProperty("source")]
        public Guid SourceId { get; }

        [Required]
        [JsonProperty("target")]
        public Guid TargetId { get; }

        private Edge() : base() { }

        private Edge([DisallowNull] Edge other)
            : base(other)
        {
            this.SourceId = other.SourceId;
            this.TargetId = other.TargetId;
            this.IsDirected = other.IsDirected;
        }

        public Edge([DisallowNull] Node source, [DisallowNull] Node target)
            : this(source.Id, target.Id, false)
        {
        }

        public Edge([DisallowNull] Node source, [DisallowNull] Node target, bool isDirected)
            : this(source.Id, target.Id, isDirected)
        {
        }

        [JsonConstructor]
        public Edge(Guid sourceId, Guid targetId, bool isDirected)
            : base()
        {
            this.SourceId = sourceId;
            this.TargetId = targetId;
            this.IsDirected = isDirected;
        }

        [Pure]
        public override object Clone()
        {
            return new Edge(this);
        }

        [Pure]
        public bool Equals(Edge other)
        {
            return other != null
                && this.SourceId.Equals(other.SourceId)
                && this.TargetId.Equals(other.TargetId)
                && this.IsDirected == other.IsDirected;
        }

        [Pure]
        public bool Equals(Edge x, Edge y)
        {
            return x != null && x.Equals(y) || y == null;
        }

        [Pure]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Edge);
        }

        [Pure]
        public int GetHashCode([DisallowNull] Edge obj)
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
