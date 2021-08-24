using Graphs.Elements;
using System;
using System.Linq;

namespace Game.Controller.ActionHandlers
{
    public sealed class MoveActionHandler
        : IActionHandler
    {
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

            if (verb.Attribute("name") != "go")
            {
                throw new ArgumentException(nameof(verb));
            }

            var location = graph
               .Where<Node>(player, 1, n => n.Is("location"))
               .Select(f => f.node)
               .Single();

            // date will act as transction if computer crashes before decouple of current location fails
            _ = graph.Couple(player, target)
                .Classify("current")
                .Qualify("since", DateTime.UtcNow.ToString("o"));

            if (!graph.TryDecouple(player, location, out var egde))
            {
                throw new InvalidOperationException("failed to decouple player from current location");
            }
        }
    }
}
