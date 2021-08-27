using Game.Controller.Exceptions;
using Graphs.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Controller
{
    internal sealed class CommandParser
    {
        private readonly IGraph graph;
        private readonly Node player;

        public CommandParser(IGraph graph, Node player)
        {
            this.graph = graph ?? throw new ArgumentNullException(nameof(graph));
            this.player = player ?? throw new ArgumentNullException(nameof(player));
        }

        public Command Parse(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException($"'{nameof(input)}' cannot be null or whitespace.", nameof(input));
            }

            var parts = input
                .ToLowerInvariant()
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 2)
            {
                throw new ArgumentException(input);
            }

            var location = this.graph
               .Where<Node>(this.player, 1, n => n.Is("location"))
               .Select(f => f.node)
               .Single();

            var accessibleLocations = this.GetAccessibleLocations(location);
            var allowedObjects = this.GetAllowedObjects(location);

            var verb = this.GetVerb(parts, input, allowedObjects, accessibleLocations);
            var handler = this.GetHandler(verb);

            var targetName = parts.Single(p => p != verb.Attribute("name"));
            var target = this.GetTarget(targetName, verb, allowedObjects, accessibleLocations);

            return new Command(verb, target, handler);
        }

        private IEnumerable<Node> GetAccessibleLocations(Node location)
        {
            return this.graph
                .Where(location, 1, n => n.Is("location"), e => e.Is("path"))
                .Select(f => f.node)
                .Append(location);
        }

        private IEnumerable<Node> GetAllowedObjects(Node location)
        {
            var inventory = this.graph
                .Where<Node>(this.player, 1, n => n.Is("inventory"))
                .Select(f => f.node)
                .Single();

            var inventoryItems = this.graph
                .Where(inventory, 1, n => n.Is("object"), e => e.Is("contains"))
                .Select(f => f.node)
                .Append(inventory);

            // todo: depth of 2 lets player see into containers, but if a container is closed they should have to open it first
            return this.graph
                .Where(location, 2, n => n.Is("object"), e => e.Is("contains"))
                .Select(f => f.node)
                .Union(inventoryItems)
                .Distinct();
        }

        private Node GetHandler(Node verb)
        {
            return this.graph
                .Where<Node>(verb, 1, n => n.Is("handler"))
                .Select(f => f.node)
                .Single();
        }

        private Node GetVerb(
            string[] parts,
            string input,
            IEnumerable<Node> allowedObjects,
            IEnumerable<Node> accessibleLocations)
        {
            var allowedObjectActions = allowedObjects
                .SelectMany(o => this.graph.Where<Node>(o, 1, n => n.Is("action")))
                .Select(f => f.node);

            var allowedLocationActions = accessibleLocations
                .SelectMany(o => this.graph.Where<Node>(o, 1, n => n.Is("action")))
                .Select(f => f.node);

            var allowedActions = allowedObjectActions
                .Union(allowedLocationActions)
                .Distinct();

            var verb = allowedActions
                .FirstOrDefault(a => parts.Contains(a.Attribute("name")));

            return verb ?? throw new ActionNotFoundException(input);
        }

        private Node GetTarget(
            string targetName,
            Node verb,
            IEnumerable<Node> allowedObjects,
            IEnumerable<Node> accessibleLocations)
        {
            var allowedTargets = allowedObjects
                .Union(accessibleLocations)
                .Where(t => this.graph.Where<Node>(t, 1, v => v.Is("action") && v == verb).Any())
                .Distinct();

            var target = allowedTargets
                .FirstOrDefault(a => targetName == a.Attribute("name"));

            return target ?? throw new ActionTargetNotFoundException(targetName);
        }
    }
}
