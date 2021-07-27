using System;

namespace graph.lib
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
        }

        public string Label { get; }

        public bool HasLabel => !String.IsNullOrEmpty(this.Label);
    }
}
