using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Graphs.Elements
{
    [DebuggerDisplay("{Id} {Source} {Target}")]
    [JsonObject("edge")]
    public sealed class Edge
        : Element
        , IEnumerable<Guid>
        , IEquatable<Edge>
        , IEqualityComparer<Edge>
    {
        private Edge()
            : base()
        {
        }

        public Edge(
            Node source,
            Node target)
            : base()
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            this.Source = source.Id;
            this.Target = target.Id;
        }

        public Edge(
            Guid source,
            Guid target)
            : base()
        {
            this.Source = source;
            this.Target = target;
        }

        public Edge(
            Guid id,
            Guid source,
            Guid target)
            : base(id)
        {
            this.Source = source;
            this.Target = target;
        }

        public Edge(
            Node source,
            Node target,
            bool isDirected)
            : base()
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            this.Source = source.Id;
            this.Target = target.Id;
            this.IsDirected = isDirected;
        }

        public Edge(
            Guid source,
            Guid target,
            bool isDirected)
            : base()
        {
            this.Source = source;
            this.Target = target;
            this.IsDirected = isDirected;
        }

        public Edge(
            Guid id,
            Guid source,
            Guid target,
            bool isDirected)
            : base(id)
        {
            this.Source = source;
            this.Target = target;
            this.IsDirected = isDirected;
        }

        private Edge(Edge other)
            : base(other)
        {
            this.Source = other.Source;
            this.Target = other.Target;
            this.IsDirected = other.IsDirected;
        }

        [JsonProperty("directed")]
        public bool IsDirected { get; private set; }

        [JsonProperty("souce")]
        public Guid Source { get; private set; }

        [JsonProperty("target")]
        public Guid Target { get; private set; }

        public IEnumerator<Guid> GetEnumerator()
        {
            yield return this.Source;
            yield return this.Target;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Edge);
        }

        public bool Equals(Edge other)
        {
            return other != null
                && other.Source == this.Source
                && other.Target == this.Target
                && other.IsDirected == this.IsDirected;
        }

        public bool Equals(Edge x, Edge y)
        {
            return x != null && x.Equals(y) || y == null;
        }

        public int GetHashCode([DisallowNull] Edge obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Source, this.Target, this.IsDirected);
        }

        public override object Clone()
        {
            return new Edge(this);
        }
    }
}
