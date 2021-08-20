using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph.Indexes
{
    public abstract class AdjacencyList<TKey>
        : AdjacencyIndex<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        protected readonly Dictionary<TKey, HashSet<TKey>> Index;

        protected AdjacencyList()
        {
            this.Index = new();
        }

        protected AdjacencyList(AdjacencyList<TKey> other)
        {
            this.Index = new Dictionary<TKey, HashSet<TKey>>(other.Index);
        }

        public override int Degree(TKey node)
        {
            return this.Index.TryGetValue(node, out var neighbors) && neighbors != null
                ? neighbors.Count
                : 0;
        }

        public override TKey First()
        {
            return this.Size > 0
                ? this.Index.Keys.First()
                : throw new InvalidOperationException("First is invalid on empty index.");
        }

        public override IEnumerable<TKey> Neighbors(TKey node)
        {
            if (this.Index.TryGetValue(node, out var neighbors) && neighbors != null)
            {
                foreach (var neigbhor in neighbors)
                {
                    yield return neigbhor;
                }
            }
        }

        public override int Size => this.Index.Count;

        public override string ToMatrix()
        {
            var builder = new StringBuilder();
            foreach(var outerkey in this.Index.Keys)
            {
                foreach (var innerkey in this.Index.Keys)
                {
                    builder.Append(this.Adjacent(outerkey, innerkey) ? 1 : 0);
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
