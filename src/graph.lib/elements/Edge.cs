using System;
using System.Collections;
using System.Collections.Generic;

namespace graph.elements
{
    public class Edge
        : Element, IEnumerable<Vertex>
    {
        private readonly Vertex[] vertices = new Vertex[2];

        private Edge() { }

        public Edge(
            Vertex v1,
            Vertex v2)
            : base()
        {
            if (v1 is null)
            {
                throw new ArgumentNullException(nameof(v1));
            }

            if (v2 is null)
            {
                throw new ArgumentNullException(nameof(v2));
            }

            this.vertices[0] = v1;
            this.vertices[1] = v2;
        }

        public Edge(
            Vertex v1,
            Vertex v2,
            IEnumerable<string> classifications)
            : base(classifications)
        {
            if (v1 is null)
            {
                throw new ArgumentNullException(nameof(v1));
            }

            if (v2 is null)
            {
                throw new ArgumentNullException(nameof(v2));
            }

            this.vertices[0] = v1;
            this.vertices[1] = v2;
        }

        public Edge(
            Vertex v1,
            Vertex v2,
            IEnumerable<string> classifications,
            Dictionary<string, object> features)
            : base(classifications, features)
        {
            if (v1 is null)
            {
                throw new ArgumentNullException(nameof(v1));
            }

            if (v2 is null)
            {
                throw new ArgumentNullException(nameof(v2));
            }

            this.vertices[0] = v1;
            this.vertices[1] = v2;
        }

        public Vertex Vertex1 => this.vertices[0];

        public Vertex Vertex2 => this.vertices[1];

        public IEnumerator<Vertex> GetEnumerator()
        {
            foreach (var vertex in this.vertices)
            {
                yield return vertex;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
