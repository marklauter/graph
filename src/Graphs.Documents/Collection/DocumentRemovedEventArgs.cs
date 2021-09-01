namespace Graphs.Documents
{
    public class DocumentRemovedEventArgs<T>
        : DocumentEventArgs<T>
        where T : class
    {
        public DocumentRemovedEventArgs(Document<T> document)
            : base(document)
        {
        }
    }
}
