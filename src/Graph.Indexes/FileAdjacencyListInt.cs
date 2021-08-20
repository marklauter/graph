using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graphs.Indexes
{
    public abstract class FileAdjacencyListInt
        : AdjacencyIndex<int>
    {
        public string Path { get; }

        private int size;
        public override int Size => this.size;

        protected FileAdjacencyListInt(string path)
        {
            this.Path = !String.IsNullOrEmpty(path) ? path : "data";

            if (!Directory.Exists(this.Path))
            {
                Directory.CreateDirectory(this.Path);
            }

            this.size = Directory.EnumerateFiles(this.Path)
                .Count();
        }

        protected string ClonePath()
        {
            var newPath = $"{this.Path}.clone.{Guid.NewGuid()}";
            Directory.CreateDirectory(newPath);
            foreach (var fileName in Directory.EnumerateFiles(this.Path))
            {
                File.Copy(
                    fileName,
                    System.IO.Path.Combine(newPath, System.IO.Path.GetFileName(fileName)));
            }

            return newPath;
        }

        public override bool Adjacent(int source, int target)
        {
            var fileName = System.IO.Path.Combine(this.Path, source.ToString());
            return File.Exists(fileName) && FileContainsKey(fileName, target);
        }

        public override int Degree(int node)
        {
            var fileName = System.IO.Path.Combine(this.Path, node.ToString());
            if (File.Exists(fileName))
            {
                using var file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                using var reader = new BinaryReader(file);

                return reader.ReadInt32();
            }

            return 0;
        }

        public override int First()
        {
            return this.Size > 0
                ? Int32.Parse(Directory.EnumerateFiles(this.Path).First())
                : throw new InvalidOperationException("First is invalid on empty index.");
        }

        public override IEnumerable<int> Keys()
        {
            return Directory
                .EnumerateFiles(this.Path)
                .Select(f => Int32.Parse(System.IO.Path.GetFileName(f)));
        }

        public override IEnumerable<int> Neighbors(int node)
        {
            var fileName = System.IO.Path.Combine(this.Path, node.ToString());
            using var file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(file);

            var length = reader.ReadInt32();
            for (var i = 0; i < length; ++i)
            {
                yield return reader.ReadInt32();
            }
        }

        protected static bool FileContainsKey(string fileName, int key)
        {
            using var file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(file);

            var length = reader.ReadInt32();
            for (var i = 0; i < length; ++i)
            {
                if (key == reader.ReadInt32())
                {
                    return true;
                }
            }

            return false;
        }

        protected void AddKeyToFile(string fileName, bool fileExists, int key)
        {
            if (fileExists)
            {
                using var file = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
                using var reader = new BinaryReader(file);
                using var writer = new BinaryWriter(file);
                var length = reader.ReadInt32() + 1;
                writer.Seek(0, SeekOrigin.Begin);
                writer.Write(length);
                writer.Seek(0, SeekOrigin.End);
                writer.Write(key);
            }
            else
            {
                using var file = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
                using var writer = new BinaryWriter(file);
                writer.Seek(0, SeekOrigin.Begin);
                writer.Write(1);
                writer.Write(key);
                ++this.size;
            }
        }

        protected void RemoveKeyFromFile(string fileName, int key)
        {
            using var file = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
            using var reader = new BinaryReader(file);

            var length = reader.ReadInt32();
            if (length - 1 == 0)
            {
                reader.Close();
                File.Delete(fileName);
                --this.size;

                return;
            }

            var keys = new int[length - 1];
            var deletionPoint = 0;
            var index = 0;
            for (var i = 0; i < length; ++i)
            {
                var value = reader.ReadInt32();
                if (key != value)
                {
                    keys[index++] = value;
                }
                else
                {
                    deletionPoint = i;
                }
            }

            using var writer = new BinaryWriter(file);
            writer.Seek(0, SeekOrigin.Begin);
            writer.Write(length - 1);

            var offset = deletionPoint * sizeof(int) + sizeof(int);
            writer.Seek(offset, SeekOrigin.Begin);
            for (var i = deletionPoint; i < length - 1; ++i)
            {
                writer.Write(keys[i]);
            }
        }
    }
}
