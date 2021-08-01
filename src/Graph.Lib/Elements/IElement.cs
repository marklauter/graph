using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Graph.Elements
{
    public interface IElement
    {
        public Guid Id { get; }

        public ImmutableHashSet<string> Labels { get; }
        public void Classify(string label);
        public void Classify(IEnumerable<string> labels);
        public void Declassify(string label);
        public bool Is(string label);

        public IImmutableDictionary<string, object> Attributes { get; }
        public void Qualify(string key, object value);
        public void Qualify(IDictionary<string, object> attributes);
        public object Attribute(string key);
    }
}
