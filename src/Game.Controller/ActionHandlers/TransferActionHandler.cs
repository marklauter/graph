using Graphs.Elements;
using System;
using System.Linq;

namespace Game.Controller.ActionHandlers
{
    internal sealed class TransferActionHandler
        : IActionHandler
    {
        public event EventHandler<ActionHandledEventArgs> ActionHandled;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3358:Ternary operators should not be nested", Justification = "<Pending>")]
        public void HandleAction(
            IGraph graph,
            Node player,
            Command command)
        {
            if (graph is null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            if (player is null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var requiredAttributeName = command.Handler.Attribute("requiredAttribute");

            if (!command.Verb.HasAttribute(requiredAttributeName))
            {
                throw new ArgumentException($"Verb {command.Verb.Attribute("name")} is missing required attribute {command.Handler.Attribute("requiredAttribute")}");
            }

            var currentOwner = graph
               .Where(command.Target, 1, n => n.Is("location") || n.Is("container"), e => e.Is("contains"))
               .Select(f => f.node)
               .Single();

            var newOwner = command.Verb.Attribute(requiredAttributeName) == "fromPlayer"
                ? graph
                   .Where<Node>(player, 1, n => n.Is("location"))
                   .Select(f => f.node)
                   .Single()
                : command.Verb.Attribute(requiredAttributeName) == "toPlayer"
                    ? graph
                        .Where<Node>(player, 1, n => n.Is("inventory"))
                        .Select(f => f.node)
                        .Single()
                    : throw new ArgumentException($"Verb {command.Verb.Attribute("name")} contains invalid value '{command.Verb.Attribute(requiredAttributeName)}' for required attribute '{command.Handler.Attribute("requiredAttribute")}'");

            // todo: when direction is toPlayer, check the sum of weight of objects in inventory + weight of new object against max weight of inventory to prevent over encumbering player
            _ = graph.Couple(newOwner, command.Target)
                .Classify("contains")
                .Qualify("since", DateTime.UtcNow.ToString("o"));

            if (!graph.TryDecouple(currentOwner, command.Target, out var egde))
            {
                throw new InvalidOperationException($"failed to decouple {command.Target.Attribute("name")} from {currentOwner.Attribute("name")}");
            }

            this.ActionHandled?.Invoke(this, new ActionHandledEventArgs($"owernship of {command.Target.Attribute("name")} transfered from {currentOwner.Attribute("name")} to {newOwner.Attribute("name")}."));
        }
    }
}
