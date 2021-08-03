using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Indexes
{
    public abstract class AdjacencyList
        : AdjacencyIndex
    {
        protected readonly HashSet<int>[] Index;

        protected AdjacencyList()
        {
            this.Index = Array.Empty<HashSet<int>>();
            this.Size = 0;
        }

        protected AdjacencyList(AdjacencyList other, int size)
        {
            this.Index = new HashSet<int>[size];

            for (var i = size - 1; i >= other.Size; --i)
            {
                this.Index[i] = new HashSet<int>();
            }

            for (var i = other.Size - 1; i >= 0; --i)
            {
                this.Index[i] = other.Index[i];
            }

            this.Size = size;
        }

        public override int Degree(int vertex)
        {
            return this.Index[vertex].Count;
        }

        public override int[] Neighbors(int vertex)
        {
            return this.Index[vertex].ToArray();
        }
    }
}
