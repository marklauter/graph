using Graph.Graphs;
using System.Collections.Generic;

namespace Graph.Indexes
{
    public interface IAdjacencyIndex<TKey>
        : IEnumerable<TKey>
    {
        public bool Adjacent(TKey vertex1, TKey vertex2);

        public TKey[] BreadthFirstSearch(TKey vertex);

        public IAdjacencyIndex<TKey> Clone();

        public void Connect(TKey vertex1, TKey vertex2);

        public int Degree(TKey vertex);

        public TKey[] DepthFirstSearchPostOrder(TKey vertex);

        public TKey[] DepthFirstSearchPreOrder(TKey vertex);

        public void Disconnect(TKey vertex1, TKey vertex2);

        public TKey[] Neighbors(TKey vertex);

        public IAdjacencyIndex<TKey> Resize(TKey size);

        public TKey Size { get; }

        public GraphType Type { get; }
    }
}
