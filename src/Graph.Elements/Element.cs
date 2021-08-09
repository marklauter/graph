﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Graph.Elements
{
    [DebuggerDisplay("{Id}")]
    public abstract class Element
        : IElement
    {
        protected Element() { }

        protected Element(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            this.Id = id;
        }

        protected Element(Element other)
        {
            // todo: copy labels
            // todo: copy attributes
        }

        [JsonProperty("id")]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [JsonProperty("labels")]
        public ImmutableHashSet<string> Labels { get; private set; } = ImmutableHashSet<string>.Empty;

        [JsonProperty("attributes")]
        public IImmutableDictionary<string, string> Attributes { get; private set; } = ImmutableDictionary<string, string>.Empty;

        public string Attribute(string attribute)
        {
            return this.Attributes.TryGetValue(attribute, out var value)
                ? value
                : null;
        }

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

        public abstract object Clone();

        public void Declassify(string label)
        {
            if (String.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException($"'{nameof(label)}' cannot be null or whitespace.", nameof(label));
            }

            this.Labels = this.Labels.Remove(label);
        }

        public bool Has(string attribute)
        {
            return this.Attributes.ContainsKey(attribute);
        }

        public bool Is(string label)
        {
            return this.Labels.Contains(label);
        }

        public void Qualify(string attribute, string value)
        {
            if (String.IsNullOrWhiteSpace(attribute))
            {
                throw new ArgumentException($"'{nameof(attribute)}' cannot be null or whitespace.", nameof(attribute));
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"'{nameof(value)}' cannot be null or whitespace.", nameof(value));
            }

            this.Attributes = this.Attributes.SetItem(attribute, value);
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
    }
}
