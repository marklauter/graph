using Graph.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Graph.Sets
{
    public sealed class KeyedSet<T>
            : Element, IEnumerable<T>
            where T : Element
    {
        private ImmutableDictionary<Guid, T> elements = ImmutableDictionary<Guid, T>.Empty;

        private KeyedSet() : base() { }

        private KeyedSet(IEnumerable<T> elements)
            : this()
        {
            this.elements = elements.ToImmutableDictionary(e => e.Id, e => e);
        }

        public static readonly KeyedSet<T> Empty = new();

        public static KeyedSet<T> Create(IEnumerable<T> elements)
        {
            return new KeyedSet<T>(elements);
        }

        //protected IImmutableDictionary<Guid, T> ElementIndex => this.elements;

        internal IEnumerable<T> Elements => this.elements.Values;

        public T this[Guid elementId] => this.elements[elementId];

        public int Size => this.elements.Count;

        public KeyedSet<T> Add(T element)
        {
            this.elements = this.elements.Add(element.Id, element);
            return this;
        }

        public KeyedSet<T> AddRange(IEnumerable<T> elements)
        {
            this.elements = this.elements.AddRange(elements.ToImmutableDictionary(e => e.Id, e => e));
            return this;
        }

        public KeyedSet<T> Remove(Guid elementId)
        {
            this.elements = this.elements.Remove(elementId);
            return this;
        }

        public KeyedSet<T> Remove(T element)
        {
            return this.Remove(element.Id);
        }

        public KeyedSet<T> Union(KeyedSet<T> other)
        {
            return new KeyedSet<T>(this.Elements.Union(other.Elements));
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

        public static explicit operator T[](KeyedSet<T> set)
        {
            return set.elements.Values.ToArray();
        }

        public static explicit operator KeyedSet<T>(T[] elements)
        {
            return new KeyedSet<T>(elements);
        }
    }
}
