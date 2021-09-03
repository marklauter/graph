using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Graphs.Documents.IO
{
    internal sealed class DocumentActionQueueProcessor<T>
        : IDisposable
        where T : class
    {
        // queues minimize write collisions on files better than locks and also improve perceived performance

        private readonly ConcurrentQueue<DocumentActionItem<T>> documentActionQueue = new();
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly DeleteDocument deleteDocument;
        private readonly WriteDocument writeDocument;
        private bool disposedValue;

        public delegate void DeleteDocument(string key);
        public delegate void WriteDocument(Document<T> document);

        public DocumentActionQueueProcessor(
            DeleteDocument deleteDocument,
            WriteDocument writeDocument)
        {
            this.StartActionQueueProcessor();
            this.deleteDocument = deleteDocument ?? throw new ArgumentNullException(nameof(deleteDocument));
            this.writeDocument = writeDocument ?? throw new ArgumentNullException(nameof(writeDocument));
        }

        public void EnqueueAddAction(Document<T> document)
        {
            this.documentActionQueue.Enqueue(new DocumentActionItem<T>(DocumentAction.Add, document));
        }

        public void EnqueueRemoveAction(string key)
        {
            this.documentActionQueue.Enqueue(new DocumentActionItem<T>(DocumentAction.Remove, key));
        }

        public void EnqueueUpdateAction(Document<T> document)
        {
            this.documentActionQueue.Enqueue(new DocumentActionItem<T>(DocumentAction.Update, document));
        }

        public void Flush()
        {
            while (this.documentActionQueue.TryDequeue(out var actionItem))
            {
                this.ProcessActionItem(actionItem);
            }
        }

        private void ProcessActionItem(DocumentActionItem<T> actionItem)
        {
            switch (actionItem.Action)
            {
                case DocumentAction.Add:
                case DocumentAction.Update:
                    this.writeDocument.Invoke(actionItem.Item as Document<T>);
                    break;
                case DocumentAction.Remove:
                    this.deleteDocument.Invoke(actionItem.Item as string);
                    break;
            }
        }

        private void ProcessActionQueue(CancellationToken cancellationToken)
        {
            var wait = new SpinWait();
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!this.documentActionQueue.TryDequeue(out var actionItem))
                {
                    wait.SpinOnce();
                    continue;
                }

                this.ProcessActionItem(actionItem);
            }

            this.Flush();
        }

        private void StartActionQueueProcessor()
        {
            // todo: make this actually run in a thread: https://www.minatcoding.com/blog/tech-tips/tech-tip-creating-a-long-running-background-task-in-net-core/
            this.ProcessActionQueue(this.cancellationTokenSource.Token);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.cancellationTokenSource.Cancel();
                }

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
