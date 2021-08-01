using System;
using System.Collections.Generic;

namespace Graph.Sets
{
    internal sealed class NonDirectedAdjacencyList
        : IGraph<int>
    {
        private readonly List<int>[] index;

        public static readonly NonDirectedAdjacencyList Empty = new();

        public int Size { get; }

        private NonDirectedAdjacencyList()
        {
            this.index = Array.Empty<List<int>>();
            this.Size = 0;
        }

        private NonDirectedAdjacencyList(NonDirectedAdjacencyList other, int size)
        {
            this.index = new List<int>[size];
            
            for (var i = size - 1; i >= other.Size; --i)
            {
                this.index[i] = new List<int>();
            }

            for (var i = other.Size - 1; i >= 0; --i)
            {
                this.index[i] = new List<int>(other.index[i]);
            }

            this.Size = size;
        }

        public NonDirectedAdjacencyList Resize(int size)
        {
            return new NonDirectedAdjacencyList(this, size);
        }

        public void Connect(int vertex1, int vertex2)
        {
            throw new NotImplementedException();
        }

        public void Connect(int vertex1, int vertex2, byte weight)
        {
            throw new NotImplementedException();
        }

        public void Disconnect(int vertex1, int vertex2)
        {
            throw new NotImplementedException();
        }

        public int Degree(int vertex)
        {
            throw new NotImplementedException();
        }

        public bool Adjacent(int vertex1, int vertex2)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> Neighbors(int vertex)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> DepthFirstSearchPreOrder(int vertex)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> DepthFirstSearchPostOrder(int vertex)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> BreadthFirstSearch(int vertex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<int> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
