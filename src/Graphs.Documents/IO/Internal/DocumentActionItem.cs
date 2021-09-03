using System;

namespace Graphs.Documents.IO
{
    internal sealed class DocumentActionItem<T>
        where T : class
    {
        public DocumentActionItem(DocumentAction action, string key)
        {
            this.Item = key ?? throw new ArgumentNullException(nameof(key));
            this.Action = action;
        }

        public DocumentActionItem(DocumentAction action, Document<T> document)
        {
            this.Item = document ?? throw new ArgumentNullException(nameof(document));
            this.Action = action;
        }

        public object Item { get; }

        public DocumentAction Action { get; }
    }
}
