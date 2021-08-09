using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Indexes
{
    public abstract class AdjacencyMatrix
        : AdjacencyIndex<int>
    {
        protected bool[,] Matrix { get; private set; }

        protected AdjacencyMatrix()
        {
            this.Matrix = new bool[0, 0];
            this.size = 0;
        }

        protected AdjacencyMatrix(AdjacencyMatrix other)
        {
            this.Matrix = new bool[other.size, other.size];
            for (var o = other.size - 1; o >= 0; --o)
            {
                for (var i = other.size - 1; i >= 0; --i)
                {
                    this.Matrix[o, i] = other.Matrix[o, i];
                }
            }

            this.size = (int)Math.Pow(this.Matrix.Length, 1 / (double)this.Matrix.Rank);
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

            var newSize = (int)(minSize + 1 + minSize * 0.10);
            var matrix = new bool[newSize, newSize];

            for (var o = this.size - 1; o >= 0; --o)
            {
                for (var i = this.size - 1; i >= 0; --i)
                {
                    matrix[o, i] = this.Matrix[o, i];
                }
            }

            this.Matrix = matrix;
            this.size = (int)Math.Pow(this.Matrix.Length, 1 / (double)this.Matrix.Rank);
        }

        private int size;
        public override int Size => this.size;

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var o = this.Size - 1; o >= 0; --o)
            {
                for (var i = this.Size - 1; i >= 0; --i)
                {
                    builder.Append(this.Matrix[o, i]);
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
