using System;

namespace Graphs.Documents
{
    public class DocumentEventArgs<T>
        : EventArgs
        where T : class
    {
        public DocumentEventArgs(Document<T> document)
        {
            this.Document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public Document<T> Document { get; }
    }
}
