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

            var parts = input.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 2)
            {
                throw new ArgumentException(input);
            }

            var verb = this.GetVerb(parts, input);
            var target = this.GetTarget(parts, input);

            return new Command(verb, target);
        }

        private Node GetVerb(string[] parts, string input)
        {
            var location = this.graph.Neighbors(this.player)
                .Single(n => n.Is("location"));

            var inventory = this.graph.Neighbors(this.player) // todo: make relationship queryable
                .Where(n => n.Is("")); // todo: where relationship is inventory

            var objects = this.graph.Neighbors(location)
                .Union(inventory);

            var actions = // todo: traverse a set of nodes 1 level deep to fetch the related actions - see the MIT video for frontier based BFS

            for (var i = 0; i < parts.Length; ++i)
            {
                if (this.actions.TryGetValue(parts[i], out var verb))
                {
                    return verb;
                }
            }

            throw new ActionNotFoundException(input);
        }

        private Node GetTarget(string[] parts, string input)
        {
            for (var i = 0; i < parts.Length; ++i)
            {
                if (this.objects.TryGetValue(parts[i], out var verb))
                {
                    return verb;
                }
            }

            throw new ObjectNotFoundException(input);
        }
    }
}
