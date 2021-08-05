using System;
using System.Collections.Generic;

namespace Graph.Indexes
{
    public abstract class BinaryAdjacencyMatrix
        : AdjacencyIndex<int>
    {
        protected ulong[,] Matrix { get; private set; }

        protected BinaryAdjacencyMatrix()
        {
            this.Matrix = new ulong[0, 0];
            this.size = 0;
        }

        protected BinaryAdjacencyMatrix(BinaryAdjacencyMatrix other)
        {
            this.Matrix = new ulong[other.size, other.size];
            Array.Copy(other.Matrix, this.Matrix, other.size);
            this.size = this.Matrix.Length;
        }

        public override int Degree(int vertex)
        {
            var degree = 0;
            for (var i = this.size - 1; i >= 0; --i)
            {
                if (this.Adjacent(vertex, i))
                {
                    ++degree;
                }
            }

            return degree;
        }

        public override int[] Neighbors(int vertex)
        {
            var neighbors = new List<int>();
            for (var i = this.size - 1; i >= 0; --i)
            {
                if (this.Adjacent(vertex, i))
                {
                    neighbors.Add(i);
                }
            }

            return neighbors.ToArray();
        }

        protected void Grow(int minSize)
        {
            if (minSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minSize));
            }

            var newSize = (int)(((double)minSize / sizeof(ulong) + 1) + (double)minSize / sizeof(ulong) * 0.10);
            var matrix = new ulong[newSize, newSize];

            Array.Copy(this.Matrix, matrix, this.size);

            this.Matrix = matrix;
            this.size = (int)Math.Pow(this.Matrix.Length, 1 / (double)this.Matrix.Rank);
        }

        private int size;
        public override int Size => this.size;

        //public override string ToString()
        //{
        //    var builder = new StringBuilder();
        //    for (var o = this.Size - 1; o >= 0; --o)
        //    {
        //        for (var i = this.Size - 1; i >= 0; --i)
        //        {
        //            builder.Append(this.Matrix[o, i]);
        //        }
        //        builder.AppendLine();
        //    }

        //    return builder.ToString();
        //}
    }
}
