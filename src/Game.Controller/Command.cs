using Graphs.Elements;

namespace Game.Controller
{
    public sealed class Command
    {
        public Command(Node verb, Node target)
        {
            this.Verb = verb;
            this.Target = target;
        }

        /// <summary>
        /// Verb is the action being performed by the subject.
        /// The subject is always the player.
        /// </summary>
        public Node Verb { get; }

        /// <summary>
        /// Target is the direct object of the verb.
        /// </summary>
        public Node Target { get; }
    }
}
