﻿using Graph.Graphs;

namespace Graph.Indexes
{
    public sealed class DirectedAdjacencyMatrix
        : AdjacencyMatrix
    {
        public static IAdjacencyIndex<int> Empty { get; } = new DirectedAdjacencyMatrix();

        public override GraphType Type => GraphType.Directed;

        private DirectedAdjacencyMatrix()
            : base()
        {
        }

        private DirectedAdjacencyMatrix(AdjacencyMatrix other, int size)
            : base(other, size)
        {
        }

        public override bool Adjacent(int vertex1, int vertex2)
        {
            return this.Matrix[vertex1, vertex2] > 0;
        }

        public override void Connect(int vertex1, int vertex2)
        {
            this.Matrix[vertex1, vertex2] = 1;
        }

        public override void Disconnect(int vertex1, int vertex2)
        {
            this.Matrix[vertex1, vertex2] = 0;
        }

        public override IAdjacencyIndex<int> Resize(int size)
        {
            return new DirectedAdjacencyMatrix(this, size);
        }
    }
}
