using graph.elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace graph.sets
{
    public class Set<T>
        : Element, IEnumerable<T>
        where T : Element
    {
        private ImmutableDictionary<Guid, T> elements = ImmutableDictionary<Guid, T>.Empty;

        private Set() : base() { }

        public static readonly Set<T> Empty = new();

        public Set(IEnumerable<string> classifications)
            : base(classifications)
        {
        }

        public Set(IEnumerable<string> classifications, IEnumerable<T> elements)
            : this(classifications)
        {
            this.elements = elements.ToImmutableDictionary(e => e.Id, e => e);
        }

        protected IImmutableDictionary<Guid, T> ElementIndex => this.elements;

        internal IEnumerable<T> Elements => this.elements.Values;

        public T this[Guid elementId] => this.elements[elementId];

        public void Add(T element)
        {
            this.elements = this.elements.Add(element.Id, element);
        }

        public T Remove(Guid elementId)
        {
            if (this.elements.TryGetValue(elementId, out var value))
            {
                this.elements = this.elements.Remove(elementId);
                return value;
            }
            else
            {
                return default;
            }
        }

        public T Remove(T element)
        {
            return this.Remove(element.Id);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var element in this.elements.Values)
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
