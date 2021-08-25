using Game.Controller;
using Graphs.Elements;
using Graphs.IO;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Game.Adventure
{
    internal sealed class Application
    {
        private readonly IConfigurationRoot config;
        private readonly IRepository<Graph> repository = new JsonRepository<Graph>("Data");

        public Application()
        {
            this.config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();
        }

        private Graph LoadGame(Guid graphId)
        {
            return (Graph)this.repository.Read(graphId);
        }

        public void Run()
        {
            var graphId = Guid.Parse(this.config["graphId"]);
            var graph = this.LoadGame(graphId);
            var game = graph.FirstOrDefault(n => n.Is("game"));
            var player = graph
                .Where<Node>(game, 1, n => n.Is("player"))
                .Select(f => f.node)
                .Single();

            var controller = new GameController(graph, game, player);

            Console.WriteLine($"Hello, {player.Attribute("name")}. Welcome to {game.Attribute("name")} ver {game.Attribute("version")}.");
            Console.WriteLine("Good luck guessing how the command system works and try not to die. <evil laugh>");
            Console.WriteLine("");

            while (true)
            {
                var location = graph
                    .Where<Node>(player, 1, n => n.Is("location"))
                    .Select(f => f.node)
                    .First();

                Console.WriteLine($"You're located here: {location.Attribute("name")}.");

                var accessibleLocations = graph
                    .Where<Node>(location, 1, n => n.Is("location"))
                    .Select(f => f.node);

                Console.WriteLine();
                Console.WriteLine($"You can see the following:");
                var i = 0;
                foreach (var place in accessibleLocations)
                {
                    Console.WriteLine($"{++i}. {place.Attribute("name")}");
                }

                Console.WriteLine();
                Console.WriteLine("What do you want to do?");
                var success = false;
                while (!success)
                {
                    var input = Console.ReadLine();
                    try
                    {
                        controller.ProcessCommand(input);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        var color = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Nope. Guess again. (ex: {ex.GetType()} - '{ex.Message}')");
                        Console.ForegroundColor = color;
                    }
                }

                this.repository.Update((Entity<Graph>)graph);
                Console.WriteLine("---------------------------");
                Console.Beep();
            }
        }
    }
}
