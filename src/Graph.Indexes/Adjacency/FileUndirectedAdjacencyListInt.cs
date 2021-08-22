using System.IO;

namespace Graphs.Indexes
{
    public sealed class FileUndirectedAdjacencyListInt
        : FileAdjacencyListInt
    {
        public override IndexType Type { get; } = IndexType.Undirected;

        public FileUndirectedAdjacencyListInt(string path)
            : base(path)
        {
        }

        public override object Clone()
        {
            return new FileUndirectedAdjacencyListInt(this.ClonePath());
        }

        public override bool Couple(int source, int target)
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

        public override bool Decouple(int source, int target)
        {
            var fileName = System.IO.Path.Combine(this.Path, source.ToString());
            if (File.Exists(fileName) && FileContainsKey(fileName, target))
            {
                this.RemoveKeyFromFile(fileName, target);

                fileName = System.IO.Path.Combine(this.Path, target.ToString());
                if (File.Exists(fileName))
                {
                    this.RemoveKeyFromFile(fileName, source);
                }

                return true;
            }

            return false;
        }
    }
}
