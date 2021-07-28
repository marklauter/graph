using System.Collections.Generic;

namespace graph.elements
{
    public class Vertex
        : Element
    {
        public Vertex() { }

        public Vertex(IEnumerable<string> classifications)
            : base(classifications)
        {
        }

        public Vertex(
            IEnumerable<string> classifications,
            Dictionary<string, object> features)
            : base(classifications, features)
        {
        }
    }
}
