using System;
using System.Collections.Generic;

namespace Graphs.Documents
{
    public class DocumentsEventArgs<T>
        : EventArgs
        where T : class
    {
        public DocumentsEventArgs(IEnumerable<Document<T>> documents)
        {
            this.Documents = documents ?? throw new ArgumentNullException(nameof(documents));
        }

        public IEnumerable<Document<T>> Documents { get; }
    }
}
