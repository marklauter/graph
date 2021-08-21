using Graphs.Elements;
using System;

namespace Game.Controller.Exceptions
{
    public sealed class IllegalActionException
        : Exception
    {
        public IllegalActionException(Node verb, Node subject)
        {
            this.Verb = verb ?? throw new ArgumentNullException(nameof(verb));
            this.Subject = subject ?? throw new ArgumentNullException(nameof(subject));
        }

        public Node Verb { get; }

        public Node Subject { get; }
    }
}
