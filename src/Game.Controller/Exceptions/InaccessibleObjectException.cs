using Graphs.Elements;
using System;

namespace Game.Controller.Exceptions
{
    public sealed class InaccessibleObjectException
        : Exception
    {
        public InaccessibleObjectException(Node subject)
        {
            this.Subject = subject ?? throw new ArgumentNullException(nameof(subject));
        }

        public Node Subject { get; }
    }
}
