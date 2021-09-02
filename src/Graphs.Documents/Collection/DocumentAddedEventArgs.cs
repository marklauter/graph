namespace Graphs.Documents
{
    public class DocumentAddedEventArgs<T>
        : DocumentEventArgs<T>
        where T : class
    {
        public DocumentAddedEventArgs(Document<T> document)
            : base(document)
        {
        }
    }
}
