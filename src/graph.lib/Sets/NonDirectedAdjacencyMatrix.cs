using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Graph.Sets
{
    public sealed class NonDirectedAdjacencyMatrix
        : IGraph<int>
    {
        private readonly byte[,] matrix;

        public static NonDirectedAdjacencyMatrix Empty { get; } = new();

        public int Size { get; }

        private NonDirectedAdjacencyMatrix()
        {
            this.matrix = new byte[0, 0];
            this.Size = 0;
        }

        private NonDirectedAdjacencyMatrix(NonDirectedAdjacencyMatrix other, int size)
        {
            this.matrix = new byte[size, size];

            for (var o = other.Size - 1; o >= 0; --o)
            {
                for (var i = other.Size - 1; i >= 0; --i)
                {
                    this.matrix[o, i] = other.matrix[o, i];
                }
            }

            this.Size = (int)Math.Pow(this.matrix.Length, 1 / (double)this.matrix.Rank);
        }

        public NonDirectedAdjacencyMatrix Resize(int size)
        {
            return new NonDirectedAdjacencyMatrix(this, size);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var o = this.Size - 1; o >= 0; --o)
            {
                for (var i = this.Size - 1; i >= 0; --i)
                {
                    builder.Append(this.matrix[o, i]);
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }

        public void Connect(int vertex1, int vertex2)
        {
            this.matrix[vertex1, vertex2] = 1;
            this.matrix[vertex2, vertex1] = 1;
        }

        public void Connect(int vertex1, int vertex2, byte weight)
        {
            this.matrix[vertex1, vertex2] = weight;
            this.matrix[vertex2, vertex1] = weight;
        }

        public void Disconnect(int vertex1, int vertex2)
        {
            this.matrix[vertex1, vertex2] = 0;
            this.matrix[vertex2, vertex1] = 0;
        }

        public int Degree(int vertex)
        {
            var degree = 0;
            for (var i = this.Size - 1; i >= 0; --i)
            {
                if (this.Adjacent(vertex, i))
                {
                    ++degree;
                }
            }

            return degree;
        }

        public bool Adjacent(int vertex1, int vertex2)
        {
            return this.matrix[vertex1, vertex2] > 0 || this.matrix[vertex2, vertex1] > 0;
        }

        public IEnumerable<int> Neighbors(int vertex)
        {
            var neighbors = new List<int>();
            for (var i = this.Size - 1; i >= 0; --i)
            {
                if (this.Adjacent(vertex, i))
                {
                    neighbors.Add(i);
                }
            }

            return neighbors;
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

        public IEnumerable<int> BreadthFirstSearch(int vertex)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<int> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
