using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Repositories.Locking
{
    // todo: add a thread that deletes old gates from the dictionary
    internal class NamedLocks
    {
        private readonly ConcurrentDictionary<string, GateEntry> gates = new();

        private ReaderWriterLockSlim GetGate(string key)
        {
            if (this.gates.TryGetValue(key, out var gateEntry))
            {
                gateEntry.Touch();
            }
            else
            {
                gateEntry = new GateEntry();
                this.gates.TryAdd(key, gateEntry);
            }

            return gateEntry.Gate;
        }

        public ReaderWriterLockSlim EnterReadLock(string key, TimeSpan timeout)
        {
            return this.GetGate(key).TryEnterReadLock(timeout)
                ? this.GetGate(key) // touches the gate
                : throw new TimeoutException(nameof(EnterReadLock));
        }

        public void ExitReadLock(ReaderWriterLockSlim gate)
        {
            gate.ExitReadLock();
        }

        public ReaderWriterLockSlim EnterUpgradeableReadLock(string key, TimeSpan timeout)
        {
            return this.GetGate(key).TryEnterUpgradeableReadLock(timeout)
                ? this.GetGate(key) // touches the gate
                : throw new TimeoutException(nameof(EnterUpgradeableReadLock));
        }

        public void ExitUpgradeableReadLock(ReaderWriterLockSlim gate)
        {
            gate.ExitUpgradeableReadLock();
        }

        public ReaderWriterLockSlim EnterWriteLock(string key, TimeSpan timeout)
        {
            return this.GetGate(key).TryEnterWriteLock(timeout)
                ? this.GetGate(key) // touches the gate
                : throw new TimeoutException(nameof(EnterWriteLock));
        }

        public void ExitWriteLock(ReaderWriterLockSlim gate)
        {
            gate.ExitWriteLock();
        }

        public bool IsReadLockHeld(string key)
        {
            return this.GetGate(key).IsReadLockHeld;
        }

        public bool IsUpgradeableReadLockHeld(string key)
        {
            return this.GetGate(key).IsUpgradeableReadLockHeld;
        }

        public bool IsWriteLockHeld(string key)
        {
            return this.GetGate(key).IsWriteLockHeld;
        }

        public int CurrentReadCount(string key)
        {
            return this.GetGate(key).CurrentReadCount;
        }

        public int WaitingReadCount(string key)
        {
            return this.GetGate(key).WaitingReadCount;
        }

        public int WaitingWriteCount(string key)
        {
            return this.GetGate(key).WaitingWriteCount;
        }

        public int WaitingUpgradeCount(string key)
        {
            return this.GetGate(key).WaitingUpgradeCount;
        }
    }
}
