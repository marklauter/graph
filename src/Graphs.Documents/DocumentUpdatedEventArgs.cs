namespace Graphs.Documents
{
    public class DocumentUpdatedEventArgs<T>
        : DocumentEventArgs<T>
        where T : class
    {
        public DocumentUpdatedEventArgs(Document<T> document) 
            : base(document)
        {
        }
    }
}
