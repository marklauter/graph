using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Graph.DB.Elements
{
    [DebuggerDisplay("{Id}")]
    [JsonObject("vertex")]
    public sealed class Vertex
        : Element
        , IEquatable<Vertex>
        , IEqualityComparer<Vertex>
    {
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Vertex);
        }

        public bool Equals(Vertex other)
        {
            return other != null
                && other.Id == this.Id;
        }

        public bool Equals(Vertex x, Vertex y)
        {
            return x != null && x.Equals(y) || y == null;
        }

        public int GetHashCode([DisallowNull] Vertex obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id);
        }
    }
}
