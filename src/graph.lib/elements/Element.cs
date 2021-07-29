using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Graph.Elements
{
    public abstract class Element
    {
        private ImmutableList<string> classifications = ImmutableList<string>.Empty;
        private ImmutableDictionary<string, object> features = ImmutableDictionary<string, object>.Empty;

        protected Element() { }

        public Guid Id { get; } = Guid.NewGuid();

        public IImmutableList<string> Classifications => this.classifications;

        public IImmutableDictionary<string, object> Features => this.features;

        public void Classify(string classification)
        {
            if (String.IsNullOrWhiteSpace(classification))
            {
                throw new ArgumentException($"'{nameof(classification)}' cannot be null or whitespace.", nameof(classification));
            }

            if (!this.classifications.Contains(classification))
            {
                this.classifications = this.classifications.Add(classification);
            }
        }

        public void Classify(IEnumerable<string> classifications)
        {
            if (classifications is null)
            {
                throw new ArgumentNullException(nameof(classifications));
            }

            this.classifications = this.classifications
                .Union(classifications.Where(c => !String.IsNullOrWhiteSpace(c)))
                .Distinct()
                .ToImmutableList();
        }

        public void Qualify(string key, object value)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            this.features = this.features.SetItem(key, value);
        }

        public void Qualify(IDictionary<string, object> features)
        {
            if (features is null)
            {
                throw new ArgumentNullException(nameof(features));
            }

            this.features = this.features
                .Union(features.Where(keyValue => !String.IsNullOrWhiteSpace(keyValue.Key)))
                .Distinct()
                .ToImmutableDictionary();
        }
    }
}
