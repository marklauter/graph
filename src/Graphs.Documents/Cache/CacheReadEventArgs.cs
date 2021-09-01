using System;

namespace Graphs.Documents
{
    public sealed class CacheReadEventArgs
        : EventArgs
    {
        public CacheReadEventArgs(string key, CacheReadType accessType)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            this.Key = key;
            this.ReadType = accessType;
        }

        public string Key { get; }

        public CacheReadType ReadType { get; }
    }
}
