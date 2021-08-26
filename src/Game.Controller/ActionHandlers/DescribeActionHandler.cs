using Graphs.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Controller.ActionHandlers
{
    public sealed class DescribeActionHandler
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

            var stringBuilder = new StringBuilder();

            var containedObjects = GetContainedObjects(graph, target);

            if ((target.Is("object") || target.Is("location")) && target.HasAttribute("description"))
            {
                stringBuilder
                    .AppendLine($"looking at {target.Attribute("name")} you see {target.Attribute("description")}");
            }

            if (target.Is("container") || target.Is("location"))
            {
                if (containedObjects.Any())
                {
                    var i = 1;
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine($"{target.Attribute("name")} contains:");
                    foreach (var node in containedObjects)
                    {
                        stringBuilder.AppendLine($"  {i}. {node.Attribute("name")}");
                    }
                }
                else
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine($"{target.Attribute("name")} is empty.");
                }
            }

            if (target.Is("location"))
            {
                var accessibleLocations = GetAccessibleLocations(graph, target);
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
