using System;
using System.IO;

namespace Graphs.Indexes
{
    public sealed class FileDirectedAdjacencyListGuid
        : FileAdjacencyListGuid
    {
        public override IndexType Type { get; } = IndexType.Directed;

        public FileDirectedAdjacencyListGuid(string path)
            : base(path)
        {
        }

        public override object Clone()
        {
            return new FileDirectedAdjacencyListGuid(this.ClonePath());
        }

        public override bool Couple(Guid source, Guid target)
        {
            var fileName = System.IO.Path.Combine(this.Path, source.ToString());
            var fileExists = File.Exists(fileName);
            if (!fileExists || !FileContainsKey(fileName, target))
            {
                this.AddKeyToFile(fileName, fileExists, target);
                return true;
            }

            return false;
        }

        public override bool Decouple(Guid source, Guid target)
        {
            var fileName = System.IO.Path.Combine(this.Path, source.ToString());
            if (File.Exists(fileName) || FileContainsKey(fileName, target))
            {
                this.RemoveKeyFromFile(fileName, target);
                return true;
            }

            return false;
        }
    }
}
