using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace graph.elements
{
    public abstract class Element
    {
        private readonly ImmutableSortedSet<string> classifications;
        private readonly ImmutableDictionary<string, object> attributes;

        protected Element()
        {
            this.Id = Guid.NewGuid();
            this.classifications = ImmutableSortedSet<string>.Empty;
            this.attributes = ImmutableDictionary<string, object>.Empty;
        }

        protected Element(IEnumerable<string> classifications)
            : this()
        {
            if (classifications is null)
            {
                throw new ArgumentNullException(nameof(classifications));
            }

                this.classifications = this.classifications.Create<string>();
        }

        public Guid Id { get; }

        public IImmutableSet<string> Classifications => this.classifications;

        public ImmutableDictionary<string, object> Attributes => this.attributes;

        public string this[int index]
        {
            get => this.classifications[index];
            set => this.classifications = this.classifications.Add
        }

        public object this[string key] => this.attributes[key];

        public bool TrySetAttribute(string key, object value)
        {
            return this.attributes.TryGetValue(key, out var oldValue)
                ? this.attributes.TryUpdate(key, value, oldValue)
                : this.attributes.TryAdd(key, value);
        }

        public


    }
}
