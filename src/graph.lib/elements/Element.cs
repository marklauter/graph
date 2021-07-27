using System;
using System.Collections.Generic;

namespace graph.elements
{
    public abstract class Element
    {
        protected Element() { }

        protected Element(string Label)
        {
            if (Label is null)
            {
                throw new ArgumentNullException(nameof(Label));
            }

            this.Label = Label;
            this.Attributes = new Dictionary<string, object>();
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public string Label { get; }

        public bool IsLabeled => !String.IsNullOrEmpty(this.Label);

        public Dictionary<string, object> Attributes { get; }
    }
}
