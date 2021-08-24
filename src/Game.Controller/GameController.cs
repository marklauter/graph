using Game.Controller.ActionHandlers;
using Graphs.Elements;
using System;
using System.Linq;

namespace Game.Controller
{
    internal sealed class GameController
    {
        private readonly IGraph graph;
        private readonly Node player;
        private readonly CommandParser commandParser;

        public GameController(IGraph graph, Node game)
        {
            this.graph = graph ?? throw new ArgumentNullException(nameof(graph));
            this.player = graph
                .Where<Node>(game, 1, n => n.Is("player"))
                .Select(f => f.node)
                .Single();

            this.commandParser = new CommandParser(this.graph, this.player);
        }

        public void ProcessCommand(string input)
        {
            var command = this.commandParser.Parse(input);
            var handler = this.GetHandler(command);
            handler.HandleAction(this.graph, this.player, command.Verb, command.Target);
        }

        private IActionHandler GetHandler(Command command)
        {
            var type = Type.GetType($"Game.Controller.ActionHandlers.{command.Handler.Attribute("classname")}", true, true);
            return (IActionHandler)Activator.CreateInstance(type);
        }
    }
}
