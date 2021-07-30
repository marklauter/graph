using Graph.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Graph.Sets
{
    public class Set<T>
        : Element, IEnumerable<T>
        where T : Element
    {
        private T[] elements = Array.Empty<T>();

        protected Set() : base() { }

        protected Set(IEnumerable<T> elements)
            : this()
        {
            this.elements = elements.ToArray(); // always makes a deep copy
        }

        public static readonly Set<T> Empty = new();

        public static Set<T> Create(IEnumerable<T> elements)
        {
            return new Set<T>(elements);
        }

        internal ImmutableArray<T> Elements => this.elements.ToImmutableArray();

        public T this[int index] => this.elements[index];

        public int Size => this.elements.Length;

        public Set<T> Add(T element)
        {
            this.elements = this.elements
                .Union(new T[] { element })
                .ToArray();

            return this;
        }

        public Set<T> AddRange(IEnumerable<T> elements)
        {
            this.elements = this.elements
                .Union(elements)
                .ToArray();

            return this;
        }

        public Set<T> Remove(Guid elementId)
        {
            var index = Array.FindIndex(this.elements, e => e.Id == elementId);
            this.elements[index] = null;
            this.elements = this.elements
                .Where(e => e != null)
                .ToArray();

            return this;
        }

        public Set<T> Remove(T element)
        {
            return this.Remove(element.Id);
        }

        public Set<T> Union(Set<T> other)
        {
            return new Set<T>(this.elements.Union(other.elements));
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var element in this.elements)
            {
                yield return element;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public static explicit operator T[](Set<T> set)
        {
            return set.elements.ToArray(); // return a copy
        }

        public static explicit operator Set<T>(T[] elements)
        {
            return new Set<T>(elements);
        }
    }
}
