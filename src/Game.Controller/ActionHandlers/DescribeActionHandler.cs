using Game.Controller.Exceptions;
using Graphs.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Controller.ActionHandlers
{
    internal sealed class DescribeActionHandler
        : IActionHandler
    {
        public event EventHandler<ActionHandledEventArgs> ActionHandled;

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

            var currentLocation = graph
                .Where(player, 1, n => n.Is("location"), e => e.Is("current"))
                .Select(f => f.node)
                .Single();

            if (command.Target.Is("location") && command.Target != currentLocation)
            {
                throw new IllegalActionException(command.Verb, command.Target);
            }

            var stringBuilder = new StringBuilder();

            var containedObjects = GetContainedObjects(graph, command.Target);

            if ((command.Target.Is("object") || command.Target.Is("location")) && command.Target.HasAttribute("description"))
            {
                stringBuilder
                    .AppendLine($"looking at {command.Target.Attribute("name")} you see {command.Target.Attribute("description")}");
            }

            if (command.Target.Is("container") || command.Target.Is("location"))
            {
                if (containedObjects.Any())
                {
                    var i = 1;
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine($"{command.Target.Attribute("name")} contains:");
                    foreach (var node in containedObjects)
                    {
                        stringBuilder.AppendLine($"  {i}. {node.Attribute("name")}");
                    }
                }
                else
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine($"{command.Target.Attribute("name")} is empty.");
                }
            }

            if (command.Target.Is("location"))
            {
                var accessibleLocations = GetAccessibleLocations(graph, command.Target);
                if (accessibleLocations.Any())
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine($"In the distance you see other places to explore:");
                    var i = 0;
                    foreach (var place in accessibleLocations)
                    {
                        stringBuilder.AppendLine($"  {++i}. {place.Attribute("name")}");
                    }
                }
            }

            this.ActionHandled?.Invoke(this, new ActionHandledEventArgs(stringBuilder.ToString()));
        }

        private static IEnumerable<Node> GetContainedObjects(IGraph graph, Node container)
        {

            return container.Is("container") || container.Is("location")
                ? graph
                    .Where(container, 1, n => n.Is("object"), e => e.Is("contains"))
                    .Select(f => f.node)
                : Enumerable.Empty<Node>();
        }

        private static IEnumerable<Node> GetAccessibleLocations(IGraph graph, Node location)
        {
            return graph
                .Where<Node>(location, 1, n => n.Is("location"))
                .Select(f => f.node);
        }
    }
}
