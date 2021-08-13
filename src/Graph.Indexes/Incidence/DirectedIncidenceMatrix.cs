using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Indexes
{
    public sealed class DirectedIncidenceMatrix
        : IncidenceMatrix
    {
        public override IndexType Type { get; }

        private DirectedIncidenceMatrix()
            : base()
        {
        }

        private DirectedIncidenceMatrix(IncidenceMatrix other)
            : base(other)
        {
        }

        public override bool Adjacent(int source, int target)
        {
            if (source >= this.Size)
            {
                throw new ArgumentOutOfRangeException(nameof(source));
            }

            if (target >= this.Size)
            {
                throw new ArgumentOutOfRangeException(nameof(target));
            }

            var sourceEdges = this.Edges(source)
                .Select(edge => edge.key);

            var targetEdges = this.Edges(target)
                .Select(edge => edge.key);

            return sourceEdges
                .Intersect(targetEdges)
                .Any();
        }

        public override object Clone()
        {
            return new DirectedIncidenceMatrix(this);
        }

        public override bool Couple(int source, int target)
        {
            throw new NotImplementedException();
        }

        public override bool Decouple(int source, int target)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<(int key, NodeType type)> Edges(int node)
        {
            if (node >= this.Size)
            {
                throw new ArgumentOutOfRangeException(nameof(node));
            }

            for (var e = 0; e < this.Size; ++e)
            {
                var edge = this.Matrix[node, e];
                if (edge != 0)
                {
                    yield return (e , edge > 0 ? NodeType.Source : NodeType.Target);
                }
            }
        }
    }
}
