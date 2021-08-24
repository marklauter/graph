using Game.Controller.Exceptions;
using Graphs.Elements;
using System;
using System.Linq;

namespace Game.Controller
{
    internal sealed class Controller
    {
        private readonly IGraph graph;
        private readonly CommandParser commandParser;

        public Controller(IGraph graph, Node game)
        {
            this.graph = graph ?? throw new ArgumentNullException(nameof(graph));
            var player = graph
                .Where<Node>(game, 1, n => n.Is("player"))
                .Select(f => f.node)
                .Single();

            this.commandParser = new CommandParser(this.graph, player);
        }

        public void ProcessCommand(Node player, Node location, Command command)
        {
            var inventory = this.graph.Neighbors(player)
                .Where(n => n.Is("object")); // todo: need to qualify the edges for inventory lable, but I don't have an incidence index yet

            var objects = this.graph.Neighbors(location)
                .Where(n => n.Is("object"));

            var isSubjectAccessible = inventory
                .Union(objects)
                .Contains(command.Target);

            if (!isSubjectAccessible)
            {
                throw new InaccessibleObjectException(command.Target);
            }

            var canExecute = this.graph.Neighbors(command.Target)
                .Where(n => n.Is("action"))
                .Contains(command.Verb);

            if (!canExecute)
            {
                throw new IllegalActionException(command.Verb, command.Target);
            }
        }
    }
}
