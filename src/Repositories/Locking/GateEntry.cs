using System.Threading;

namespace Repositories.Locking
{
    internal sealed class GateEntry
    {
        private int locks = 0;
        public ReaderWriterLockSlim Gate { get; } = new ReaderWriterLockSlim();

        public GateEntry AddLock()
        {
            Interlocked.Increment(ref this.locks);
            return this;
        }

        public GateEntry RemoveLock()
        {
            Interlocked.Decrement(ref this.locks);
            return this;
        }

        public int Locks()
        {
            return this.locks;
        }
    }
}
