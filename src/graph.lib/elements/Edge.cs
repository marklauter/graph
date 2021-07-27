using System;

namespace graph.elements
{
    public class Edge
        : Element
    {
        private Edge() { }

        public Edge(Vertex v1, Vertex v2)
        {
            if (v1 is null)
            {
                throw new ArgumentNullException(nameof(v1));
            }

            if (v2 is null)
            {
                throw new ArgumentNullException(nameof(v2));
            }

            this.Vertex1 = v1;
            this.Vertex2 = v2;
        }

        public Edge(Vertex v1, Vertex v2, string label)
            : base(label)
        {
            if (v1 is null)
            {
                throw new ArgumentNullException(nameof(v1));
            }

            if (v2 is null)
            {
                throw new ArgumentNullException(nameof(v2));
            }

            this.Vertex1 = v1;
            this.Vertex2 = v2;
        }

        public Vertex Vertex1 { get; }

        public Vertex Vertex2 { get; }
    }
}
