using System;

namespace Graphs.Documents.IO
{
    internal sealed class DocumentActionItem
    {
        public DocumentActionItem(object item, DocumentAction action)
        {
            this.Item = item ?? throw new ArgumentNullException(nameof(item));
            this.Action = action;
        }

        public object Item { get; }

        public DocumentAction Action { get; }
    }
}
