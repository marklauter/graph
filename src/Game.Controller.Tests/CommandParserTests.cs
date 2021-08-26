using Graphs.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Game.Controller.Tests
{
    public class CommandParserTests
    {

        private readonly Guid graphId = Guid.Parse("d1a83412-f7cc-45ec-96b8-01da7c6b1428");
        private readonly Guid gameId = Guid.Parse("da323889-8547-454b-a248-571dae95d9ea");
        private readonly Guid playerId = Guid.Parse("23fdc3f5-d4d8-4ef4-91f2-8e7f64bb5ce7");

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

            var input = "go castle";

            var parser = new CommandParser(graph, player);
            var command = parser.Parse(input);
            Assert.True(command.Verb.Is("action"));
            Assert.Equal("go", command.Verb.Attribute("name"));

            Assert.Equal("castle", command.Target.Attribute("name"));
            Assert.True(command.Target.Is("location"));

            Assert.True(command.Handler.Is("handler"));
            Assert.Equal("MoveActionHandler", command.Handler.Attribute("classname"));
        }

        [Fact]
        public void GameController_ProcessCommand_Succeeds()
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

            var input = "go castle";

            var originalLocation = graph
                .Where<Node>(player, 1, n => n.Is("location"))
               .Select(f => f.node)
               .Single();

            Assert.True(originalLocation.Is("location"));
            Assert.Equal("field", originalLocation.Attribute("name"));
    
            var controller = new GameController(graph, game, player);
            controller.ProcessCommand(input);

            var newLocation = graph
                .Where<Node>(player, 1, n => n.Is("location"))
               .Select(f => f.node)
               .Single();

            Assert.True(newLocation.Is("location"));
            Assert.Equal("castle", newLocation.Attribute("name"));
        }
    }
}
