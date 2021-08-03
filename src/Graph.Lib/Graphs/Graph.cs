using Graph.Indexes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Graph.Graphs
{
    public class Graph<TKey>
        : IGraph<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        private IAdjacencyIndex index;
        private readonly ConcurrentDictionary<TKey, int> keyToVertex = new();
        private readonly ConcurrentDictionary<int, TKey> vertexToKey = new();

        public Graph(IAdjacencyIndex index)
        {
            this.index = index;
        }

        private Graph(
            IAdjacencyIndex index,
            ConcurrentDictionary<TKey, int> keyToVertex,
            ConcurrentDictionary<int, TKey> vertexToKey)
        {
            this.index = index;
            this.keyToVertex = keyToVertex;
            this.vertexToKey = vertexToKey;
        }

        public int Size => this.index.Size;

        public GraphType Type => this.index.Type;

        public int Add(TKey vertex)
        {
            var size = this.index.Size;

            if (this.keyToVertex.TryAdd(vertex, size))
            {
                this.vertexToKey.TryAdd(size, vertex);
                this.index = this.index.Resize(size + 1);
            }

            return size;
        }

        public void AddRange(IEnumerable<TKey> vertices)
        {
            var size = this.index.Size;

            foreach (var vertex in vertices)
            {
                if (this.keyToVertex.TryAdd(vertex, size))
                {
                    _ = this.vertexToKey.TryAdd(size, vertex);
                    ++size;
                }
            }

            this.index = this.index.Resize(size);
        }

        public bool Adjacent(TKey vertex1, TKey vertex2)
        {
#pragma warning disable S3358 // Ternary operators should not be nested
            return !this.keyToVertex.TryGetValue(vertex1, out var v1)
                ? throw new KeyNotFoundException(nameof(vertex1))
                : !this.keyToVertex.TryGetValue(vertex2, out var v2)
                    ? throw new KeyNotFoundException(nameof(vertex2))
                    : this.index.Adjacent(v1, v2);
#pragma warning restore S3358 // Ternary operators should not be nested
        }

        public IEnumerable<TKey> BreadthFirstSearch(TKey vertex)
        {
            return !this.keyToVertex.TryGetValue(vertex, out var v)
                ? throw new KeyNotFoundException(nameof(vertex))
                : this.VerticesToKeys(this.index.BreadthFirstSearch(v));
        }

        public IGraph<TKey> Clone()
        {
            return new Graph<TKey>(
                this.index.Clone(),
                new ConcurrentDictionary<TKey, int>(this.keyToVertex),
                new ConcurrentDictionary<int, TKey>(this.vertexToKey));
        }

        public void Connect(TKey vertex1, TKey vertex2)
        {
            if (!this.keyToVertex.TryGetValue(vertex1, out var v1))
            {
                v1 = this.Add(vertex1);
            }

            if (!this.keyToVertex.TryGetValue(vertex2, out var v2))
            {
                v2 = this.Add(vertex1);
            }

            this.index.Connect(v1, v2);
        }

        public int Degree(TKey vertex)
        {
            return !this.keyToVertex.TryGetValue(vertex, out var v)
                ? throw new KeyNotFoundException(nameof(vertex))
                : this.index.Degree(v);
        }

        public IEnumerable<TKey> DepthFirstSearchPostOrder(TKey vertex)
        {
            return !this.keyToVertex.TryGetValue(vertex, out var v)
                ? throw new KeyNotFoundException(nameof(vertex))
                : this.VerticesToKeys(this.index.DepthFirstSearchPostOrder(v));
        }

        public IEnumerable<TKey> DepthFirstSearchPreOrder(TKey vertex)
        {
            return !this.keyToVertex.TryGetValue(vertex, out var v)
                ? throw new KeyNotFoundException(nameof(vertex))
                : this.VerticesToKeys(this.index.DepthFirstSearchPreOrder(v));
        }

        public void Disconnect(TKey vertex1, TKey vertex2)
        {
            if (!this.keyToVertex.TryGetValue(vertex1, out var v1))
            {
                throw new KeyNotFoundException(nameof(vertex1));
            }

            if (!this.keyToVertex.TryGetValue(vertex2, out var v2))
            {
                throw new KeyNotFoundException(nameof(vertex2));
            }

            this.index.Disconnect(v1, v2);
        }

        public IEnumerable<TKey> Neighbors(TKey vertex)
        {
            return !this.keyToVertex.TryGetValue(vertex, out var v)
                ? throw new KeyNotFoundException(nameof(vertex))
                : this.VerticesToKeys(this.index.Neighbors(v));
        }

        private TKey[] VerticesToKeys(int[] vertices)
        {
            var keys = new TKey[vertices.Length];

            for (var i = vertices.Length - 1; i >= 0; --i)
            {
                keys[i] = this.vertexToKey[vertices[i]];
            }

            return keys;
        }
    }
}
