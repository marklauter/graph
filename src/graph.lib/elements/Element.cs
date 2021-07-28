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
            this.classifications = this.classifications.Add(classification);
        }

        public void Classify(IEnumerable<string> classifications)
        {
            this.classifications = this.classifications
                .Union(classifications)
                .ToImmutableList();
        }

        public void Qualify(string key, object value)
        {
            this.features = this.features.SetItem(key, value);
        }

        public void Qualify(IDictionary<string, object> features)
        {
            this.features = this.features
                .Union(features)
                .ToImmutableDictionary();
        }
    }
}
