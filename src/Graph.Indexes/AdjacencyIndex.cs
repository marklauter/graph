using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Graphs.Indexes
{
    public abstract class AdjacencyIndex
    {
        public static IndexType ParseType(string matrix)
        {
            if (String.IsNullOrWhiteSpace(matrix))
            {
                throw new ArgumentException($"'{nameof(matrix)}' cannot be null or whitespace.", nameof(matrix));
            }

            var reader = new StringReader(matrix);
            var line = reader.ReadLine();

            return (IndexType)Enum.Parse(typeof(IndexType), line);
        }
    }

    public abstract class AdjacencyIndex<TKey>
        : AdjacencyIndex
        , IAdjacencyIndex<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public abstract bool Adjacent(TKey source, TKey target);

        public abstract bool Couple(TKey source, TKey target);

        public abstract object Clone();

        public abstract bool Decouple(TKey source, TKey target);

        public abstract int Degree(TKey node);

        public abstract TKey First();

        public abstract IEnumerable<TKey> Keys();

        public abstract IEnumerable<TKey> Neighbors(TKey node);

        public void Parse(string matrix)
        {
            if (String.IsNullOrWhiteSpace(matrix))
            {
                throw new ArgumentException($"'{nameof(matrix)}' cannot be null or whitespace.", nameof(matrix));
            }

            var reader = new StringReader(matrix);
            _ = reader.ReadLine(); // discard type

            var lines = new Dictionary<TKey, string>();
            var line = reader.ReadLine();
            while (!String.IsNullOrEmpty(line))
            {
                var parts = line.Split(':');
                if(parts.Length < 2)
                {
                    throw new FormatException(line);
                }

                var key = (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(parts[0]);
                // var key = (TKey)Convert.ChangeType(parts[0], typeof(TKey));
                lines.Add(key, parts[1]);
                line = reader.ReadLine();
            }

            foreach(var outerkey in lines.Keys)
            {
                var i = 0;
                line = lines[outerkey];
                foreach (var innerkey in lines.Keys)
                {
                    if (line[i] == '1')
                    {
                        this.Couple(outerkey, innerkey);
                    }

                    ++i;
                }
            }
        }
        
        public abstract int Size { get; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(this.Type);
            builder.AppendLine();
            foreach (var outerkey in this.Keys())
            {
                builder.Append(outerkey);
                builder.Append(':');
                foreach (var innerkey in this.Keys())
                {
                    builder.Append(this.Adjacent(outerkey, innerkey) ? 1 : 0);
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        public abstract IndexType Type { get; }
    }
}
