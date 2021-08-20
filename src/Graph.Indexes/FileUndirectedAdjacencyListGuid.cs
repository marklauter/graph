using System;
using System.IO;

namespace Graphs.Indexes
{
    public sealed class FileUndirectedAdjacencyListGuid
        : FileAdjacencyListGuid
    {
        public override IndexType Type { get; } = IndexType.Undirected;

        public FileUndirectedAdjacencyListGuid(string path)
            : base(path)
        {
        }

        public override object Clone()
        {
            return new FileUndirectedAdjacencyListGuid(this.ClonePath());
        }

        public override bool Couple(Guid source, Guid target)
        {
            var fileName = System.IO.Path.Combine(this.Path, source.ToString());
            var fileExists = File.Exists(fileName);
            if (!fileExists || !FileContainsKey(fileName, target))
            {
                this.AddKeyToFile(fileName, fileExists, target);
                
                fileName = System.IO.Path.Combine(this.Path, target.ToString());
                fileExists = File.Exists(fileName);
                this.AddKeyToFile(fileName, fileExists, source);
                
                return true;
            }

            return false;
        }

        public override bool Decouple(Guid source, Guid target)
        {
            var fileName = System.IO.Path.Combine(this.Path, source.ToString());
            if (File.Exists(fileName)  || FileContainsKey(fileName, target))
            {
                this.RemoveKeyFromFile(fileName, target);

                fileName = System.IO.Path.Combine(this.Path, target.ToString());
                if(File.Exists(fileName))
                {
                    this.RemoveKeyFromFile(fileName, source);
                }

                return true;
            }

            return false;
        }
    }
}
