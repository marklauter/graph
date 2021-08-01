using System.Collections;
using System.Collections.Generic;

namespace Graph.Sets
{
    public abstract class Graph
        : IGraph
    {
        public IEnumerable<int> DepthFirstSearchPostOrder(int vertex)
        {
            var vertices = new int[this.Size];
            var visitStack = new Stack<int>();
            var visited = new bool[this.Size];
            var neighbors = new Stack<int>(new int[] { vertex });

            while (neighbors.Count > 0)
            {
                var v = neighbors.Pop();
                if (!visited[v])
                {
                    visitStack.Push(v);
                    visited[v] = true;
                    for (var i = this.Size - 1; i >= 0; --i)
                    {
                        if (i != v && this.Adjacent(v, i) && !visited[i])
                        {
                            neighbors.Push(i);
                        }
                    }
                }
            }

            for (var i = 0; i < vertices.Length; ++i)
            {
                vertices[i] = visitStack.Pop();
            }

            return vertices;
        }

        public IEnumerable<int> DepthFirstSearchPreOrder(int vertex)
        {
            var vertices = new int[this.Size];
            var visited = new bool[this.Size];
            var neighbors = new Stack<int>(new int[] { vertex });
            var vi = 0;

            while (neighbors.Count > 0)
            {
                var v = neighbors.Pop();
                if (!visited[v])
                {
                    vertices[vi] = v;
                    ++vi;
                    visited[v] = true;
                    for (var i = this.Size - 1; i >= 0; --i)
                    {
                        if (i != v && this.Adjacent(v, i) && !visited[i])
                        {
                            neighbors.Push(i);
                        }
                    }
                }
            }

            return vertices;
        }

        public abstract bool Adjacent(int vertex1, int vertex2);

        public IEnumerable<int> BreadthFirstSearch(int vertex)
        {
            throw new System.NotImplementedException();
        }

        public IGraph Clone()
        {
            return this.Resize(this.Size);
        }

        public abstract void Connect(int vertex1, int vertex2);

        public abstract int Degree(int vertex);

        public abstract void Disconnect(int vertex1, int vertex2);

        public IEnumerator<int> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public abstract IEnumerable<int> Neighbors(int vertex);

        public abstract IGraph Resize(int size);

        public int Size { get; protected set; }

        public abstract GraphType Type { get; }
    }
}
