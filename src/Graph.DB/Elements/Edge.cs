using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Graph.DB.Elements
{
    [DebuggerDisplay("{Id} {Vertex1} {Vertex2}")]
    [JsonObject]
    public sealed class Edge
        : Element
        , IEnumerable<Guid>
        , IEquatable<Edge>
        , IEqualityComparer<Edge>
    {
        private Edge() { }

        public Edge(
            Vertex vertex1,
            Vertex vertex2)
            : base()
        {
            if (vertex1 is null)
            {
                throw new ArgumentNullException(nameof(vertex1));
            }

            if (vertex2 is null)
            {
                throw new ArgumentNullException(nameof(vertex2));
            }

            this.Vertex1 = vertex1.Id;
            this.Vertex2 = vertex2.Id;
        }

        public Edge(
            Guid vertex1,
            Guid vertex2)
            : base()
        {
            this.Vertex1 = vertex1;
            this.Vertex2 = vertex2;
        }

        [JsonProperty("vertex1")]
        public Guid Vertex1 { get; private set; }

        [JsonProperty("vertex2")]
        public Guid Vertex2 { get; private set; }

        public IEnumerator<Guid> GetEnumerator()
        {
            yield return this.Vertex1;
            yield return this.Vertex2;
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
                && other.Id == this.Id
                && other.Vertex1 == this.Vertex1
                && other.Vertex2 == this.Vertex2;
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
            return HashCode.Combine(this.Id, this.Vertex1, this.Vertex2);
        }
    }
}
