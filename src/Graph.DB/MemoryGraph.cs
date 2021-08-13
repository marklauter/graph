//using Graph.Indexes;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;

//namespace Graph.Graphs
//{
//    public class MemoryGraph<TKey>
//        : IGraph<TKey>
//        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
//    {
//        private IGraphIndex<int> index;
//        private readonly ConcurrentDictionary<TKey, int> keyToVertex = new();
//        private readonly ConcurrentDictionary<int, TKey> vertexToKey = new();

//        public MemoryGraph(IGraphIndex<int> index)
//        {
//            this.index = index;
//        }

//        private MemoryGraph(
//            IGraphIndex<int> index,
//            ConcurrentDictionary<TKey, int> keyToVertex,
//            ConcurrentDictionary<int, TKey> vertexToKey)
//        {
//            this.index = index;
//            this.keyToVertex = keyToVertex;
//            this.vertexToKey = vertexToKey;
//        }

//        public int Size => this.index.Size;

//        public GraphType Type => this.index.Type;

//        public int Add(TKey vertex)
//        {
//            var size = this.index.Size;

//            if (this.keyToVertex.TryAdd(vertex, size))
//            {
//                this.vertexToKey.TryAdd(size, vertex);
//                this.index = this.index.Resize(size + 1);
//            }

//            return size;
//        }

//        public void AddRange(IEnumerable<TKey> vertices)
//        {
//            var size = this.index.Size;

//            foreach (var vertex in vertices)
//            {
//                if (this.keyToVertex.TryAdd(vertex, size))
//                {
//                    _ = this.vertexToKey.TryAdd(size, vertex);
//                    ++size;
//                }
//            }

//            this.index = this.index.Resize(size);
//        }

//        public bool Adjacent(TKey source, TKey target)
//        {
//#pragma warning disable S3358 // Ternary operators should not be nested
//            return !this.keyToVertex.TryGetValue(source, out var v1)
//                ? throw new KeyNotFoundException(nameof(source))
//                : !this.keyToVertex.TryGetValue(target, out var v2)
//                    ? throw new KeyNotFoundException(nameof(target))
//                    : this.index.Adjacent(v1, v2);
//#pragma warning restore S3358 // Ternary operators should not be nested
//        }

//        public IEnumerable<TKey> BreadthFirstSearch(TKey vertex)
//        {
//            return !this.keyToVertex.TryGetValue(vertex, out var v)
//                ? throw new KeyNotFoundException(nameof(vertex))
//                : this.VerticesToKeys(this.index.BreadthFirstSearch(v));
//        }

//        public IGraph<TKey> Clone()
//        {
//            return new MemoryGraph<TKey>(
//                this.index.Clone(),
//                new ConcurrentDictionary<TKey, int>(this.keyToVertex),
//                new ConcurrentDictionary<int, TKey>(this.vertexToKey));
//        }

//        public void Connect(TKey source, TKey target)
//        {
//            if (!this.keyToVertex.TryGetValue(source, out var v1))
//            {
//                v1 = this.Add(source);
//            }

//            if (!this.keyToVertex.TryGetValue(target, out var v2))
//            {
//                v2 = this.Add(source);
//            }

//            this.index.Couple(v1, v2);
//        }

//        public int Degree(TKey vertex)
//        {
//            return !this.keyToVertex.TryGetValue(vertex, out var v)
//                ? throw new KeyNotFoundException(nameof(vertex))
//                : this.index.Degree(v);
//        }

//        public IEnumerable<TKey> DepthFirstSearchPostOrder(TKey vertex)
//        {
//            return !this.keyToVertex.TryGetValue(vertex, out var v)
//                ? throw new KeyNotFoundException(nameof(vertex))
//                : this.VerticesToKeys(this.index.DepthFirstSearchPostOrder(v));
//        }

//        public IEnumerable<TKey> DepthFirstSearchPreOrder(TKey vertex)
//        {
//            return !this.keyToVertex.TryGetValue(vertex, out var v)
//                ? throw new KeyNotFoundException(nameof(vertex))
//                : this.VerticesToKeys(this.index.DepthFirstSearchPreOrder(v));
//        }

//        public void Disconnect(TKey source, TKey target)
//        {
//            if (!this.keyToVertex.TryGetValue(source, out var v1))
//            {
//                throw new KeyNotFoundException(nameof(source));
//            }

//            if (!this.keyToVertex.TryGetValue(target, out var v2))
//            {
//                throw new KeyNotFoundException(nameof(target));
//            }

//            this.index.Decouple(v1, v2);
//        }

//        public IEnumerable<TKey> Neighbors(TKey vertex)
//        {
//            return !this.keyToVertex.TryGetValue(vertex, out var v)
//                ? throw new KeyNotFoundException(nameof(vertex))
//                : this.VerticesToKeys(this.index.Neighbors(v));
//        }

//        private TKey[] VerticesToKeys(int[] vertices)
//        {
//            var keys = new TKey[vertices.Length];

//            for (var i = vertices.Length - 1; i >= 0; --i)
//            {
//                keys[i] = this.vertexToKey[vertices[i]];
//            }

//            return keys;
//        }
//    }
//}
