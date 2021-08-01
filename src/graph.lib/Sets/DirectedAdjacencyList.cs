﻿using System;

namespace Graph.Sets
{
    public sealed class DirectedAdjacencyList
    : AdjacencyList
    {
        public static IGraph Empty { get; } = new DirectedAdjacencyList();

        public override GraphType Type => GraphType.Directed;

        private DirectedAdjacencyList()
            : base()
        {
        }

        private DirectedAdjacencyList(AdjacencyList other, int size)
            : base(other, size)
        {
        }

        public override bool Adjacent(int vertex1, int vertex2)
        {
            return this.Index[vertex1].Contains(vertex2);
        }

        public override void Connect(int vertex1, int vertex2)
        {
            this.Index[vertex1].Add(vertex2);
        }

        public override void Disconnect(int vertex1, int vertex2)
        {
            this.Index[vertex1].Remove(vertex2);
        }

        public override IGraph Resize(int size)
        {
            return new DirectedAdjacencyList(this, size);
        }
    }

}