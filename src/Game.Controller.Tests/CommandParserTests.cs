using Graphs.Elements;
using System;
using System.Collections.Generic;
using Xunit;

namespace Game.Controller.Tests
{
    public class CommandParserTests
    {
        private readonly Guid graphId = Guid.Parse("ddbe8848-5fd4-423c-bee0-7a5b939f0bd0");
        private readonly Guid gameId = Guid.Parse("c7b4688e-62a9-4f55-a304-0caddfe2b7a7");
        private readonly Guid playerId = Guid.Parse("6446727c-1955-4a53-9fb6-297b14f7dc5a");

        [Fact]
        public void CommandParser_Parse_Succeeds()
        {
            var graph = GraphFactory.CreateGraph(this.graphId);

            if (!graph.TryGetElement<Node>(this.gameId, out var game))
            {
                throw new KeyNotFoundException("game not found");
            }

            if (!graph.TryGetElement<Node>(this.playerId, out var player))
            {
                throw new KeyNotFoundException("player not found");
            }

            var input = "look field";

            var parser = new CommandParser(graph, player);
            var command = parser.Parse(input);


        }
    }
}
