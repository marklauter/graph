﻿using System;

namespace Graph.Sets
{
    public sealed class UnDirectedAdjacencyList
        : AdjacencyList
    {
        public static IGraph Empty { get; } = new UnDirectedAdjacencyList();

        public override GraphType Type => GraphType.Undirected;

        private UnDirectedAdjacencyList()
            : base()
        {
        }

        private UnDirectedAdjacencyList(AdjacencyList other, int size)
            : base(other, size)
        {
        }

        public override bool Adjacent(int vertex1, int vertex2)
        {
            return this.Index[vertex1].Contains(vertex2)
                && this.Index[vertex2].Contains(vertex1);
        }

        public override void Connect(int vertex1, int vertex2)
        {
            this.Index[vertex1].Add(vertex2);
            this.Index[vertex2].Add(vertex1);
        }

        public override void Connect(int vertex1, int vertex2, byte weight)
        {
            throw new NotImplementedException();
        }

        public override void Disconnect(int vertex1, int vertex2)
        {
            this.Index[vertex1].Remove(vertex2);
            this.Index[vertex2].Remove(vertex1);
        }

        public override IGraph Resize(int size)
        {
            return new UnDirectedAdjacencyList(this, size);
        }
    }

}
