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
            var location = graph
                .Where<Node>(player, 1, n => n.Is("location"))
                .Select(f => f.node)
                .First();

            var controller = new GameController(graph, game, player);
            controller.ActionHandled += this.Controller_ActionHandled;

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine($"Hello, {player.Attribute("name")}. Welcome to {game.Attribute("name")} ver {game.Attribute("version")}.");
            Console.WriteLine("Good luck guessing how the command system works. Tip, check github for source code.");
            Console.WriteLine("");
            Console.WriteLine($"You're located here: {location.Attribute("name")}. try not to die, scrub. <evil laugh>");

            while (true)
            {
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
                        Console.WriteLine($"Nope. Try using a verb and direct object. Words are hard. Keep at it.");
                        Console.WriteLine($"Here's a useless hint from the developer. ex: {ex.GetType()}, '{ex.Message}'");
                        Console.ForegroundColor = color;
                    }
                }

                this.repository.Update((Entity<Graph>)graph);
                Console.WriteLine("---------------------------");
                Console.Beep();
            }
        }

        private void Controller_ActionHandled(object sender, Controller.ActionHandlers.ActionHandledEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine(e.Message);
        }
    }
}
