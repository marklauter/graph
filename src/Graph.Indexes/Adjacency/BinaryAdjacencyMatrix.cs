using System;
using System.Collections.Generic;

namespace Graph.Indexes
{
    public abstract class BinaryAdjacencyMatrix
        : GraphIndex<int>
    {
        protected ulong[][] Matrix { get; private set; }

        protected bool Read(int source, int target)
        {
            var col = Math.DivRem(target, (sizeof(ulong) * 64), out var bit);
            return (this.Matrix[source][col] & ((ulong)1 << bit)) != 0;
        }

        protected void Toggle(int source, int target)
        {
            var col = Math.DivRem(target, (sizeof(ulong) * 64), out var bit);
            this.Matrix[source][col] ^= (ulong)1 << bit;
        }

        protected BinaryAdjacencyMatrix()
        {
            this.Matrix = Array.Empty<ulong[]>();
            this.size = 0;
        }

        protected BinaryAdjacencyMatrix(BinaryAdjacencyMatrix other)
        {
            this.Matrix = new ulong[other.size][];
            for (var i = other.size - 1; i >= 0; --i)
            {
                this.Matrix[i] = new ulong[other.Matrix[i].Length];
                Array.Copy(other.Matrix[i], this.Matrix[i], other.Matrix[i].Length);
            }

            this.size = this.Matrix.Length;
        }

        public override int Degree(int node)
        {
            var degree = 0;
            for (var i = this.size - 1; i >= 0; --i)
            {
                if (this.Adjacent(node, i))
                {
                    ++degree;
                }
            }

            return degree;
        }

        public override int First()
        {
            return this.size > 0
                ? 0
                : throw new InvalidOperationException("First is invalid on empty index.");
        }

        public override IEnumerable<int> Neighbors(int node)
        {
            for (var i = this.size - 1; i >= 0; --i)
            {
                if (this.Adjacent(node, i))
                {
                    yield return i;
                }
            }
        }

        protected void Grow(int minSize)
        {
            if (minSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minSize));
            }

            var newColSize = (int)(((double)minSize / sizeof(ulong) + 1) + (double)minSize / sizeof(ulong) * 0.10);
            var newRowSize = (int)(minSize + 1 + minSize * 0.10);
            var matrix = new ulong[newRowSize][];
            for (var i = matrix.Length - 1; i >= 0; --i)
            {
                matrix[i] = new ulong[newColSize];
            }

            for (var i = this.size - 1; i >= 0; --i)
            {
                for (var j = this.Matrix[i].Length - 1; j >= 0; --j)
                {
                    matrix[i][j] = this.Matrix[i][j];
                }
            }

            this.Matrix = matrix;
            this.size = newRowSize;
        }

        private int size;
        public override int Size => this.size;
    }
}
