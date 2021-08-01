using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Graph.Elements
{
    [DebuggerDisplay("{Id}")]
#pragma warning disable S4035 // Classes implementing "IEquatable<T>" should be sealed
    public abstract class Element
#pragma warning restore S4035 // Classes implementing "IEquatable<T>" should be sealed
        : IElement
        , IEquatable<Element>
        , IEqualityComparer<Element>
    {
        protected Element() { }

        public Guid Id { get; } = Guid.NewGuid();

        private ImmutableHashSet<string> labels = ImmutableHashSet<string>.Empty;

        public ImmutableHashSet<string> Labels => this.labels;

        public void Classify(string label)
        {
            if (String.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException($"'{nameof(label)}' cannot be null or whitespace.", nameof(label));
            }

            this.labels = this.labels.Add(label);
        }

        public void Classify(IEnumerable<string> labels)
        {
            if (labels is null)
            {
                throw new ArgumentNullException(nameof(labels));
            }

            this.labels = this.labels
                .Union(labels);
        }

        public void Declassify(string label)
        {
            if (String.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException($"'{nameof(label)}' cannot be null or whitespace.", nameof(label));
            }

            this.labels = this.labels.Remove(label);
        }


        public bool Is(string label)
        {
            return this.labels.Contains(label);
        }

        private ImmutableDictionary<string, object> attributes = ImmutableDictionary<string, object>.Empty;

        public IImmutableDictionary<string, object> Attributes => this.attributes;

        public void Qualify(string key, object value)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            this.attributes = this.attributes.SetItem(key, value);
        }

        public void Qualify(IDictionary<string, object> attributes)
        {
            if (attributes is null)
            {
                throw new ArgumentNullException(nameof(attributes));
            }

            this.attributes = this.attributes
                .Union(attributes)
                .ToImmutableDictionary();
        }

        public object Attribute(string key)
        {
            return this.attributes.TryGetValue(key, out var value)
                ? value
                : null;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id);
        }

        public bool Equals(Element other)
        {
            return other != null
                && other.Id == this.Id;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Element);
        }

        public bool Equals(Element x, Element y)
        {
            return x != null && x.Equals(y) || y == null;
        }

        public int GetHashCode([DisallowNull] Element obj)
        {
            return obj.GetHashCode();
        }
    }
}
