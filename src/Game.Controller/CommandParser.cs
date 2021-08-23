using Game.Controller.Exceptions;
using Graphs.Elements;
using System;
using System.Linq;

namespace Game.Adventure
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

            var verb = this.GetVerb(parts, input);
            var targetInput = parts.Single(p => p != verb.Attribute("value"));
            var target = this.GetTarget(targetInput, verb);

            return new Command(verb, target);
        }

        private Node GetVerb(string[] parts, string input)
        {
            var location = this.graph
                .Where<Node>(this.player, 1, n => n.Is("location"))
                .Select(f => f.node)
                .Single();

            var accessibleLocations = this.graph
                .Where(location, 1, n => n.Is("location"), e => e.Is("path"))
                .Select(f => f.node);

            var inventory = this.graph
                .Where(this.player, 1, n => n.Is("object"), e => e.Is("inventory"))
                .Select(f => f.node);

            var objects = this.graph
                .Where(location, 2, n => n.Is("object"), e => e.Is("contains"))
                .Select(f => f.node)
                .Union(inventory)
                .Distinct();

            var allowedObjectActions = objects
                .SelectMany(o => this.graph.Where<Node>(o, 1, n => n.Is("action")))
                .Select(f => f.node);

            var allowedLocationActions = accessibleLocations
                .SelectMany(o => this.graph.Where<Node>(o, 1, n => n.Is("action")))
                .Select(f => f.node);

            var allowedActions = allowedObjectActions
                .Union(allowedLocationActions)
                .Distinct();

            var verb = allowedActions
                .FirstOrDefault(a => parts.Contains(a.Attribute("value")));

            return verb ?? throw new ActionNotFoundException(input);
        }

        private Node GetTarget(string targetText, Node verb)
        {
            var location = this.graph
                .Where<Node>(this.player, 1, n => n.Is("location"))
                .Select(f => f.node)
                .Single();

            var accessibleLocations = this.graph
                .Where(location, 1, n => n.Is("location"), e => e.Is("path"))
                .Select(f => f.node);

            var inventory = this.graph
                .Where(this.player, 1, n => n.Is("object"), e => e.Is("inventory"))
                .Select(f => f.node);

            var allowedObjects = this.graph
                .Where(location, 2, n => n.Is("object"), e => e.Is("contains"))
                .Select(f => f.node)
                .Union(inventory);

            var allowedTargets = allowedObjects
                .Union(accessibleLocations)
                .Where(t => this.graph.Where<Node>(t, 1, v => v.Is("action") && v == verb).Any())
                .Distinct();

            var target = allowedTargets
                .FirstOrDefault(a => targetText == a.Attribute("name"));

            return target ?? throw new ActionTargetNotFoundException(targetText);
        }
    }
}
