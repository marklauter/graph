using Graphs.Elements;

namespace Game.Controller
{
    internal sealed class Command
    {
        public Command(Node verb, Node target, Node handler)
        {
            this.Verb = verb ?? throw new System.ArgumentNullException(nameof(verb));
            this.Target = target ?? throw new System.ArgumentNullException(nameof(target));
            this.Handler = handler ?? throw new System.ArgumentNullException(nameof(handler));
        }

        public Node Handler { get; }

        /// <summary>
        /// Target is the direct object of the verb.
        /// </summary>
        public Node Target { get; }

        /// <summary>
        /// Verb is the action being performed by the subject.
        /// The subject is always the player.
        /// </summary>
        public Node Verb { get; }
    }
}
