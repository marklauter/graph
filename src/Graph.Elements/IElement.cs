using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Graphs.Elements
{
    public interface IElement
        : ICloneable
    {
        Guid Id { get; }
        
        ImmutableHashSet<string> Labels { get; }

        public IImmutableDictionary<string, string> Attributes { get; }

        public string Attribute(string attribute);
        public IElement Classify(string label);
        public IElement Classify(IEnumerable<string> labels);
        public IElement Declassify(string label);
        public bool HasAttribute(string attribute);
        public bool Is(string label);
        public IElement Qualify(string attribute, string value);
        public IElement Qualify(IDictionary<string, string> attributes);
    }
}
