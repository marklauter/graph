using System.Collections.Generic;

namespace Graphs.Documents
{
    public class DocumentsAddedEventArgs<T>
        : DocumentsEventArgs<T>
        where T : class
    {
        public DocumentsAddedEventArgs(IEnumerable<Document<T>> documents)
            : base(documents)
        {
        }
    }
}
