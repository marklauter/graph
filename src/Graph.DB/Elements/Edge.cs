﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Graph.DB.Elements
{
    [DebuggerDisplay("{Id} {Source} {Target}")]
    [JsonObject("edge")]
    public sealed class Edge
        : Element
        , IEnumerable<Guid>
        , IEquatable<Edge>
        , IEqualityComparer<Edge>
    {
        private Edge() { }

        public Edge(
            Vertex source,
            Vertex target)
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
                && other.Target == this.Target;
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
            return HashCode.Combine(this.Id, this.Source, this.Target);
        }
    }
}