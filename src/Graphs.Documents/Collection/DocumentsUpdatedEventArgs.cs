using System.Collections.Generic;

namespace Graphs.Documents
{
    public class DocumentsUpdatedEventArgs<T>
        : DocumentsEventArgs<T>
        where T : class
    {
        public DocumentsUpdatedEventArgs(IEnumerable<Document<T>> documents)
            : base(documents)
        {
        }
    }
}
