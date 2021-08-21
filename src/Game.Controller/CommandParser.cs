using Game.Controller.Exceptions;
using Graphs.Elements;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Game.Adventure
{
    internal sealed class CommandParser
    {
        private readonly ImmutableDictionary<string, Node> actions;
        private readonly ImmutableDictionary<string, Node> objects;

        public CommandParser(IGraph graph)
        {
            this.actions = graph.Nodes
                .Where(n => n.Is("action"))
                .ToImmutableDictionary(a => a.Attribute("name"));

            this.objects = graph.Nodes
                .Where(n => n.Is("object"))
                .ToImmutableDictionary(o => o.Attribute("name"));
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
