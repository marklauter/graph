using System;

namespace Graphs.Documents
{
    public class DocumentRemovedEventArgs
        : EventArgs
    {
        public DocumentRemovedEventArgs(string key)
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
