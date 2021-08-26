using Graphs.Elements;
using System;
using System.Linq;

namespace Game.Controller.ActionHandlers
{
    public sealed class MoveActionHandler
        : IActionHandler
    {
        public event EventHandler<ActionHandledEventArgs> ActionHandled;

        public void HandleAction(
            IGraph graph,
            Node player,
            Node verb,
            Node target)
        {
            if (graph is null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            if (player is null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            if (verb is null)
            {
                throw new ArgumentNullException(nameof(verb));
            }

            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            var location = graph
               .Where<Node>(player, 1, n => n.Is("location"))
               .Select(f => f.node)
               .Single();

            if (location != target)
            {
                // date will act as transction if computer crashes before decouple of current location fails
                // because to get current location if the user is linked to more than one we can just take
                // the one with the most recent date
                _ = graph.Couple(player, target)
                    .Classify("current")
                    .Qualify("since", DateTime.UtcNow.ToString("o"));

                if (!graph.TryDecouple(player, location, out var egde))
                {
                    throw new InvalidOperationException("failed to decouple player from current location");
                }

                this.ActionHandled?.Invoke(this, new ActionHandledEventArgs($"{player.Attribute("name")} moved from {location.Attribute("name")} to {target.Attribute("name")}."));
            }
            else
            {
                this.ActionHandled?.Invoke(this, new ActionHandledEventArgs($"{player.Attribute("name")} is already here: {location.Attribute("name")}."));
            }
        }
    }
}
