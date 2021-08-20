using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graph.Indexes
{
    public abstract class FileAdjacencyListGuid
        : AdjacencyIndex<Guid>
    {
        public string Path { get; }

        private int size;
        public override int Size => this.size;

        protected FileAdjacencyListGuid(string path)
        {
            this.Path = !String.IsNullOrEmpty(path) ? path : "data";

            if (!Directory.Exists(this.Path))
            {
                Directory.CreateDirectory(this.Path);
            }

            this.size = Directory
                .EnumerateFiles(this.Path)
                .Count();
        }

        protected string ClonePath()
        {
            var newPath = $"{this.Path}.clone.{Guid.NewGuid()}";
            Directory.CreateDirectory(newPath);
            foreach (var fileName in Directory.EnumerateFiles(this.Path))
            {
                File.Copy(
                    System.IO.Path.Combine(this.Path, fileName),
                    System.IO.Path.Combine(newPath, fileName));
            }

            return newPath;
        }

        public override bool Adjacent(Guid source, Guid target)
        {
            var fileName = System.IO.Path.Combine(this.Path, source.ToString());
            return File.Exists(fileName) && FileContainsKey(fileName, target);
        }

        public override int Degree(Guid node)
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

        public override Guid First()
        {
            return this.Size > 0
                ? Guid.Parse(Directory.EnumerateFiles(this.Path).First())
                : throw new InvalidOperationException("First is invalid on empty index.");
        }

        public override IEnumerable<Guid> Keys()
        {
            return Directory
                .EnumerateFiles(this.Path)
                .Select(f => Guid.Parse(System.IO.Path.GetFileName(f)));
        }

        public override IEnumerable<Guid> Neighbors(Guid node)
        {
            var fileName = System.IO.Path.Combine(this.Path, node.ToString());
            using var file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(file);

            var length = reader.ReadInt32();
            for (var i = 0; i < length; ++i)
            {
                yield return new Guid(reader.ReadBytes(16));
            }
        }

        protected static bool FileContainsKey(string fileName, Guid key)
        {
            using var file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(file);

            var length = reader.ReadInt32();
            for (var i = 0; i < length; ++i)
            {
                if (key == new Guid(reader.ReadBytes(16)))
                {
                    return true;
                }
            }

            return false;
        }

        protected void AddKeyToFile(string fileName, bool fileExists, Guid key)
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
                writer.Write(key.ToByteArray());
            }
            else
            {
                using var file = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
                using var writer = new BinaryWriter(file);
                writer.Seek(0, SeekOrigin.Begin);
                writer.Write(1);
                writer.Write(key.ToByteArray());
                ++this.size;
            }
        }

        protected void RemoveKeyFromFile(string fileName, Guid key)
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

            var keys = new byte[length - 1][];
            var deletionPoint = 0;
            var index = 0;
            for (var i = 0; i < length; ++i)
            {
                var bytes = reader.ReadBytes(16);
                if (key != new Guid(bytes))
                {
                    keys[index++] = bytes;
                }
                else
                {
                    deletionPoint = i;
                }
            }

            using var writer = new BinaryWriter(file);
            writer.Seek(0, SeekOrigin.Begin);
            writer.Write(length - 1);
            for (var i = deletionPoint; i < length - 1; ++i)
            {
                var offset = i * 16 + sizeof(int);
                writer.Seek(offset, SeekOrigin.Begin);
                writer.Write(keys[i]);
            }
        }
    }
}
