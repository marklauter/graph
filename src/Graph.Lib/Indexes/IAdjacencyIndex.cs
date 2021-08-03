using Graph.Graphs;
using System.Collections.Generic;

namespace Graph.Indexes
{
    public interface IAdjacencyIndex
        : IEnumerable<int>
    {
        public bool Adjacent(int vertex1, int vertex2);

        public int[] BreadthFirstSearch(int vertex);

        public IAdjacencyIndex Clone();

        public void Connect(int vertex1, int vertex2);

        public int Degree(int vertex);

        public int[] DepthFirstSearchPostOrder(int vertex);

        public int[] DepthFirstSearchPreOrder(int vertex);

        public void Disconnect(int vertex1, int vertex2);

        public int[] Neighbors(int vertex);

        public IAdjacencyIndex Resize(int size);

        public int Size { get; }

        public GraphType Type { get; }
    }
}
