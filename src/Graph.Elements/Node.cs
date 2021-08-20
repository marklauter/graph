using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Graphs.Elements
{
    [DebuggerDisplay("{Id}")]
    [JsonObject("node")]
    public sealed class Node
        : Element
        , IEquatable<Node>
        , IEqualityComparer<Node>
    {
        internal Node()
            : base()
        {
        }

        public Node(Guid id) : base(id) { }

        private Node(Node other)
            : base(other)
        {
        }

        public override object Clone()
        {
            return new Node(this);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Node);
        }

        public bool Equals(Node other)
        {
            return other != null
                && other.Id == this.Id;
        }

        public bool Equals(Node x, Node y)
        {
            return x != null && x.Equals(y) || y == null;
        }

        public int GetHashCode([DisallowNull] Node obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id);
        }
    }
}
