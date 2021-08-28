using System;

namespace Graphs.DB.IO
{
    public sealed class KeyEventArgs
        : EventArgs
    {
        public KeyEventArgs(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            this.Key = key;
        }

        public string Key { get; }
    }
}


