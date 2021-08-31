using System;
using System.Threading;

namespace Repositories.Locking
{
    internal sealed class GateEntry
    {
        public DateTime Touched { get; private set; } = DateTime.UtcNow;
        public ReaderWriterLockSlim Gate { get; } = new ReaderWriterLockSlim();

        public void Touch()
        {
            this.Touched = DateTime.UtcNow;
        }
    }
}
