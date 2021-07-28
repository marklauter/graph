using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace graph.elements
{
    public abstract class Element
    {
        private ImmutableSortedSet<string> classifications;
        private ImmutableDictionary<string, object> features;

        protected Element()
        {
            this.Id = Guid.NewGuid();
            this.classifications = ImmutableSortedSet<string>.Empty;
            this.features = ImmutableDictionary<string, object>.Empty;
        }

        protected Element(IEnumerable<string> classifications)
            : this()
        {
            if (classifications is null)
            {
                throw new ArgumentNullException(nameof(classifications));
            }

            // todo: when upgraded to .Net 6.0 we can use ImmutableSortedSet.Create(enumerable)
            this.classifications = classifications.ToImmutableSortedSet<string>();
        }

        protected Element(
            IEnumerable<string> classifications, 
            IDictionary<string, object> features)
            : this(classifications)
        {
            if (features is null)
            {
                throw new ArgumentNullException(nameof(features));
            }

            this.features = features.ToImmutableDictionary();
        }

        public Guid Id { get; }

        public IImmutableSet<string> Classifications => this.classifications;

        public IImmutableDictionary<string, object> Features => this.features;

        public void Classify(string classification)
        {
            this.classifications = this.classifications.Add(classification);
        }

        public void Qualify(string key, object value)
        {
            this.features = this.features.SetItem(key, value);
        }
    }
}
