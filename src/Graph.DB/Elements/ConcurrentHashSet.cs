using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Graphs.DB.Elements
{
    internal sealed class ConcurrentHashSet<T>
    {
        private readonly HashSet<T> hashset = new();
        private readonly ReaderWriterLockSlim gate = new();

        public ConcurrentHashSet() { }

        public ConcurrentHashSet(ConcurrentHashSet<T> other)
        {
            this.hashset.UnionWith(other.hashset);
        }

        public bool Add(T item)
        {
            this.gate.EnterWriteLock();
            try
            {
                return this.hashset.Add(item);
            }
            finally
            {
                this.gate.ExitWriteLock();
            }
        }

        public bool Contains(T item)
        {
            this.gate.EnterReadLock();
            try
            {
                return this.hashset.Contains(item);
            }
            finally
            {
                this.gate.ExitReadLock();
            }
        }

        public bool Remove(T item)
        {
            this.gate.EnterWriteLock();
            try
            {
                return this.hashset.Remove(item);
            }
            finally
            {
                this.gate.ExitWriteLock();
            }
        }

        public void UnionWith(IEnumerable<T> other)
        {
            this.gate.EnterWriteLock();
            try
            {
                this.hashset.UnionWith(other);
            }
            finally
            {
                this.gate.ExitWriteLock();
            }
        }

        public IEnumerable<T> Except(IEnumerable<T> second)
        {
            this.gate.EnterReadLock();
            try
            {
                return this.hashset.Except(second);
            }
            finally
            {
                this.gate.ExitReadLock();
            }
        }

        public int Count
        {
            get
            {
                this.gate.EnterReadLock();
                try
                {
                    return this.hashset.Count;
                }
                finally
                {
                    this.gate.ExitReadLock();
                }
            }
        }

        public IEnumerable<T> Items()
        {
            this.gate.EnterReadLock();
            try
            {
                return this.hashset.ToImmutableArray();
            }
            finally
            {
                this.gate.ExitReadLock();
            }
        }
    }
}
