﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Indexes
{
    public abstract class AdjacencyMatrix
        : AdjacencyIndex
    {
        protected readonly byte[,] Matrix;

        protected AdjacencyMatrix()
        {
            this.Matrix = new byte[0, 0];
            this.Size = 0;
        }

        protected AdjacencyMatrix(AdjacencyMatrix other, int size)
        {
            this.Matrix = new byte[size, size];

            for (var o = other.Size - 1; o >= 0; --o)
            {
                for (var i = other.Size - 1; i >= 0; --i)
                {
                    this.Matrix[o, i] = other.Matrix[o, i];
                }
            }

            this.Size = (int)Math.Pow(this.Matrix.Length, 1 / (double)this.Matrix.Rank);
        }

        public override int Degree(int vertex)
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

        public override int[] Neighbors(int vertex)
        {
            var neighbors = new List<int>();
            for (var i = this.Size - 1; i >= 0; --i)
            {
                if (this.Adjacent(vertex, i))
                {
                    neighbors.Add(i);
                }
            }

            return neighbors.ToArray();
        }

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
