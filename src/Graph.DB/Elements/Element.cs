using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Graph.DB.Elements
{
    [DebuggerDisplay("{Id}")]
    public abstract class Element
        : IElement
    {
        protected Element() { }

        [JsonProperty("id")]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [JsonProperty("labels")]
        public ImmutableHashSet<string> Labels { get; private set; } = ImmutableHashSet<string>.Empty;

        [JsonProperty("attributes")]
        public IImmutableDictionary<string, string> Attributes { get; private set; } = ImmutableDictionary<string, string>.Empty;

        public void Classify(string label)
        {
            if (String.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException($"'{nameof(label)}' cannot be null or whitespace.", nameof(label));
            }

            this.Labels = this.Labels.Add(label);
        }

        public void Classify(IEnumerable<string> labels)
        {
            if (labels is null)
            {
                throw new ArgumentNullException(nameof(labels));
            }

            this.Labels = this.Labels
                .Union(labels);
        }

        public void Declassify(string label)
        {
            if (String.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException($"'{nameof(label)}' cannot be null or whitespace.", nameof(label));
            }

            this.Labels = this.Labels.Remove(label);
        }

        public bool Is(string label)
        {
            return this.Labels.Contains(label);
        }

        public void Qualify(string key, object value)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.Attributes = this.Attributes.SetItem(key, value.ToString());
        }

        public void Qualify(IDictionary<string, string> attributes)
        {
            if (attributes is null)
            {
                throw new ArgumentNullException(nameof(attributes));
            }

            this.Attributes = this.Attributes
                .Union(attributes)
                .ToImmutableDictionary();
        }

        public object Attribute(string key)
        {
            return this.Attributes.TryGetValue(key, out var value)
                ? value
                : null;
        }
    }
}
