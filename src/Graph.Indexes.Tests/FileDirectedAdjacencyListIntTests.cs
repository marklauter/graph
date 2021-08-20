using System;
using System.IO;

namespace Graphs.Indexes.Tests
{
    public class FileDirectedAdjacencyListIntTests
        : DirectedAdjacencyIndexTests
        , IDisposable
    {
        private readonly string indexPath = "fileAdjacencyTests-" + Guid.NewGuid().ToString();
        private bool disposedValue;

        protected override IAdjacencyIndex<int> EmptyIndex()
        {
            return new FileDirectedAdjacencyListInt(this.indexPath);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    foreach (var folder in Directory.EnumerateDirectories("."))
                    {
                        if (folder.Contains(this.indexPath))
                        {
                            Directory.Delete(folder, true);
                        }
                    }
                }

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
