using System.Collections.Generic;

namespace Graphs.Documents
{
    public class DocumentsRemovedEventArgs<T>
        : DocumentsEventArgs<T>
        where T : class
    {
        public DocumentsRemovedEventArgs(IEnumerable<Document<T>> documents)
            : base(documents)
        {
        }
    }
}
