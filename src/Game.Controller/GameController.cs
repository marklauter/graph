using Game.Controller.ActionHandlers;
using Graphs.Elements;
using System;

namespace Game.Controller
{
    public sealed class GameController
    {
        private readonly IGraph graph;
        private readonly Node game;
        private readonly Node player;
        private readonly CommandParser commandParser;

        public event EventHandler<ActionHandledEventArgs> ActionHandled;

        public GameController(IGraph graph, Node game, Node player)
        {
            this.graph = graph ?? throw new ArgumentNullException(nameof(graph));
            this.game = game ?? throw new ArgumentNullException(nameof(game));
            this.player = player ?? throw new ArgumentNullException(nameof(player));
            this.commandParser = new CommandParser(this.graph, this.player);
        }

        public void ProcessCommand(string input)
        {
            var command = this.commandParser.Parse(input);
            var handler = CreateHandler(command);

            handler.ActionHandled += this.Handler_ActionHandled;
            handler.HandleAction(this.graph, this.player, command);
        }

        private void Handler_ActionHandled(object sender, ActionHandledEventArgs e)
        {
            (sender as IActionHandler).ActionHandled -= this.Handler_ActionHandled;
            ActionHandled?.Invoke(this, e);
        }

        private static IActionHandler CreateHandler(Command command)
        {
            var type = Type.GetType($"Game.Controller.ActionHandlers.{command.Handler.Attribute("classname")}", true, true);
            return (IActionHandler)Activator.CreateInstance(type);
        }
    }
}
