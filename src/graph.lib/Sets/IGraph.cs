using System.Collections.Generic;

namespace Graph.Sets
{
    public interface IGraph<T>
        : IEnumerable<T>
    {
        public void Connect(T vertex1, T vertex2);
        public void Connect(T vertex1, T vertex2, byte weight);

        public void Disconnect(T vertex1, T vertex2);

        public int Degree(T vertex);

        public bool Adjacent(T vertex1, T vertex2);

        public IEnumerable<T> Neighbors(T vertex);

        public IEnumerable<T> DepthFirstSearchPreOrder(T vertex);
        
        public IEnumerable<T> DepthFirstSearchPostOrder(T vertex);

        public IEnumerable<T> BreadthFirstSearch(T vertex);

        public int Size { get; }
    }
}
