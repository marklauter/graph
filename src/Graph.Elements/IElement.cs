using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Graph.Elements
{
    public interface IElement
        : ICloneable
    {
        Guid Id { get; }
        
        ImmutableHashSet<string> Labels { get; }

        public IImmutableDictionary<string, string> Attributes { get; }

        public string Attribute(string attribute);
        public void Classify(string label);
        public void Classify(IEnumerable<string> labels);
        public void Declassify(string label);
        public bool HasAttribute(string attribute);
        public bool Is(string label);
        public void Qualify(string attribute, string value);
        public void Qualify(IDictionary<string, string> attributes);
    }
}
