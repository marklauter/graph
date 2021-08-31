using System;
using System.Collections.Concurrent;

namespace Repositories.Locking
{
    // todo: add a thread that deletes old gates from the dictionary
    internal class NamedLocks
    {
        private readonly ConcurrentDictionary<string, GateEntry> gates = new();

        private GateEntry GetGate(string key)
        {
            if (this.gates.TryGetValue(key, out var gateEntry))
            {
                gateEntry = new GateEntry();
                this.gates.TryAdd(key, gateEntry);
            }

            return gateEntry;
        }

        public void EnterReadLock(string key, TimeSpan timeout)
        {
            if (!this.GetGate(key).AddLock().Gate.TryEnterReadLock(timeout))
            {
                throw new TimeoutException(nameof(EnterReadLock));
            }
        }

        public void ExitReadLock(string key)
        {
            var entry = this.GetGate(key);
            entry.RemoveLock().Gate.ExitReadLock();
            if (entry.Locks() <= 0)
            {
                _ = this.gates.TryRemove(key, out _);
            }
        }

        public void EnterUpgradeableReadLock(string key, TimeSpan timeout)
        {
            if (!this.GetGate(key).AddLock().Gate.TryEnterUpgradeableReadLock(timeout))
            {
                throw new TimeoutException(nameof(EnterUpgradeableReadLock));
            }
        }

        public void ExitUpgradeableReadLock(string key)
        {
            var entry = this.GetGate(key);
            entry.RemoveLock().Gate.ExitUpgradeableReadLock();
            if (entry.Locks() <= 0)
            {
                _ = this.gates.TryRemove(key, out _);
            }
        }

        public void EnterWriteLock(string key, TimeSpan timeout)
        {
            if (!this.GetGate(key).AddLock().Gate.TryEnterWriteLock(timeout))
            {
                throw new TimeoutException(nameof(EnterWriteLock));
            }
        }

        public void ExitWriteLock(string key)
        {
            var entry = this.GetGate(key);
            entry.RemoveLock().Gate.ExitWriteLock();
            if (entry.Locks() <= 0)
            {
                _ = this.gates.TryRemove(key, out _);
            }
        }

        public bool IsReadLockHeld(string key)
        {
            return this.gates.ContainsKey(key) && this.GetGate(key).Gate.IsReadLockHeld;
        }

        public bool IsUpgradeableReadLockHeld(string key)
        {
            return this.gates.ContainsKey(key) && this.GetGate(key).Gate.IsUpgradeableReadLockHeld;
        }

        public bool IsWriteLockHeld(string key)
        {
            return this.gates.ContainsKey(key) && this.GetGate(key).Gate.IsWriteLockHeld;
        }

        public int CurrentReadCount(string key)
        {
            return this.gates.ContainsKey(key)
                ? this.GetGate(key).Gate.CurrentReadCount
                : 0;
        }

        public int WaitingReadCount(string key)
        {
            return this.gates.ContainsKey(key)
                ? this.GetGate(key).Gate.WaitingReadCount
                : 0;
        }

        public int WaitingWriteCount(string key)
        {
            return this.gates.ContainsKey(key)
                ? this.GetGate(key).Gate.WaitingWriteCount
                : 0;
        }

        public int WaitingUpgradeCount(string key)
        {
            return this.gates.ContainsKey(key)
                ? this.GetGate(key).Gate.WaitingUpgradeCount
                : 0;
        }
    }
}
