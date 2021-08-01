using System.Collections.Generic;

namespace Graph.Sets
{
    public interface IGraph
        : IEnumerable<int>
    {
        public bool Adjacent(int vertex1, int vertex2);

        public IEnumerable<int> BreadthFirstSearch(int vertex);

        public IGraph Clone();

        public void Connect(int vertex1, int vertex2);

        public int Degree(int vertex);

        public IEnumerable<int> DepthFirstSearchPostOrder(int vertex);

        public IEnumerable<int> DepthFirstSearchPreOrder(int vertex);

        public void Disconnect(int vertex1, int vertex2);

        public IEnumerable<int> Neighbors(int vertex);

        public IGraph Resize(int size);

        public int Size { get; }

        public GraphType Type { get; }
    }
}
