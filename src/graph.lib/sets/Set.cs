using graph.elements;
using System;
using System.Collections;
using System.Collections.Generic;

namespace graph.sets
{
    public class Set<T>
        : Element, IEnumerable<T>
        where T : Element
    {
        private Set() { }

        public Set(string label)
            : base(label)
        {
            this.Elements = new Dictionary<Guid, T>();
        }

        public Set(string label, IEnumerable<T> elements)
            : this(label)
        {
            foreach (var element in elements)
            {
                this.Elements.Add(element.Id, element);
            }
        }

        protected readonly Dictionary<Guid, T> Elements;

        public T this[Guid g] => this.Elements[g];

        public IEnumerator<T> GetEnumerator()
        {
            foreach(var element in this.Elements.Values)
            {
                yield return element;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
