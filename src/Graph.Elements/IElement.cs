using System;
using System.Collections.Generic;

namespace Graph.Elements
{
    public interface IElement
        : ICloneable
    {
        Guid Id { get; }

        public string Attribute(string attribute);
        public void Classify(string label);
        public void Classify(IEnumerable<string> labels);
        public void Declassify(string label);
        public bool Has(string attribute);
        public bool Is(string label);
        public void Qualify(string attribute, string value);
        public void Qualify(IDictionary<string, string> attributes);
    }
}
