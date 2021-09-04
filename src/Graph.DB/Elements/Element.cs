using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Graphs.DB.Elements
{
    [DebuggerDisplay("{Key}")]
    public abstract class Element
        : IElement
    {
        [JsonProperty("attributes")]
        private readonly ConcurrentDictionary<string, string> attributes = new();

        [JsonProperty("labels")]
        private readonly ConcurrentHashSet<string> labels = new();

        protected Element() { }

        protected Element(Guid id)
        {
            this.Id = id;
        }

        protected Element([DisallowNull] Element other)
        {
            this.Id = other.Id;
            this.attributes = new(other.attributes);
            this.labels = new(other.labels);
        }

        /// <inheritdoc/>
        [Key]
        [Required]
        [JsonProperty("id")]
        public Guid Id { get; }

        /// <inheritdoc/>
        [Pure]
        public string Attribute(string name)
        {
            return this.attributes.TryGetValue(name, out var value)
                ? value
                : null;
        }

        /// <inheritdoc/>
        public IElement Classify(string label)
        {
            this.labels.Add(label);
            return this;
        }

        /// <inheritdoc/>
        public IElement Classify([DisallowNull] IEnumerable<string> labels)
        {
            this.labels.UnionWith(labels);
            return this;
        }

        /// <inheritdoc/>
        [Pure]
        public abstract object Clone();

        /// <inheritdoc/>
        public IElement Declassify(string label)
        {
            this.labels.Remove(label);
            return this;
        }

        /// <inheritdoc/>
        [Pure]
        public bool HasAttribute(string name)
        {
            return this.attributes.ContainsKey(name);
        }

        /// <inheritdoc/>
        [Pure]
        public bool HasLabel(string label)
        {
            return this.labels.Contains(label);
        }

        /// <inheritdoc/>
        public IElement Qualify(string name, string value)
        {
            this.attributes[name] = value;
            return this;
        }

        /// <inheritdoc/>
        public IElement Qualify([DisallowNull] IEnumerable<KeyValuePair<string, string>> attributes)
        {
            foreach (var kvp in attributes)
            {
                this.attributes[kvp.Key] = kvp.Value;
            }

            return this;
        }

        protected IEnumerable<string> GetLabels()
        {
            return this.labels;
        }
    }
}
