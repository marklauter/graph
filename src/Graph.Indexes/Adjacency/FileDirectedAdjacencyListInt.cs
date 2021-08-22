using System.IO;

namespace Graphs.Indexes
{
    public sealed class FileDirectedAdjacencyListInt
        : FileAdjacencyListInt
    {
        public override IndexType Type { get; } = IndexType.Directed;

        public FileDirectedAdjacencyListInt(string path)
            : base(path)
        {
        }

        public override object Clone()
        {
            return new FileDirectedAdjacencyListInt(this.ClonePath());
        }

        public override bool Couple(int source, int target)
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

        public override bool Decouple(int source, int target)
        {
            var fileName = System.IO.Path.Combine(this.Path, source.ToString());
            if (File.Exists(fileName) && FileContainsKey(fileName, target))
            {
                this.RemoveKeyFromFile(fileName, target);
                return true;
            }

            return false;
        }
    }
}
