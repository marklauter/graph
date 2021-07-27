using System;

namespace graph.lib
{
    public class Edge
        : Element
    {
        private Edge() { }

        public Edge(Node v1, Node v2)
        {
            if (v1 is null)
            {
                throw new ArgumentNullException(nameof(v1));
            }

            if (v2 is null)
            {
                throw new ArgumentNullException(nameof(v2));
            }

            this.V1 = v1;
            this.V2 = v2;
        }

        public Edge(Node v1, Node v2, string label)
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

            this.V1 = v1;
            this.V2 = v2;
        }

        public Node V1 { get; }
        public Node V2 { get; }
    }
}
