using System;
using System.Collections.Generic;

namespace Graph.Elements
{
    public interface IElement
    {
        Guid Id { get; }

        public object Attribute(string key);
        public void Classify(string label);
        public void Classify(IEnumerable<string> labels);
        public void Declassify(string label);
        public bool Has(string attribute);
        public bool Is(string label);
        public void Qualify(string key, object value);
        public void Qualify(IDictionary<string, string> attributes);
    }
}
