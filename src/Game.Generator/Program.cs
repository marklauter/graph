using Graphs.Elements;
using Graphs.Indexes;
using Graphs.IO;
using System;

namespace Game.Generator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var gameName = "adventure";

            var graph = new Graph(UndirectedAdjacencyList<Guid>.Empty());

            var game = (Node)graph.Add()
                .Classify("game")
                .Qualify("name", gameName)
                .Qualify("version", "0.0")
                .Qualify("sub-version", "beta");

            var player = (Node)graph.Add()
                .Classify("player")
                .Qualify("name", "adventurer")
                .Qualify("level", "1");

            var map = (Node)graph.Add()
                .Classify("map");

            graph.Couple(game, player);
            graph.Couple(game, map);

            var actionGo = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "go");

            var actionLook = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "look");

            var actionUse = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "use");

            var actionRead = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "read");

            var actionTake = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "take");

            var actionDrop = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "drop");

            var actionOpen = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "open");

            var actionClose = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "close");

            graph.Couple(game, actionGo).Classify("action");
            graph.Couple(game, actionLook).Classify("action");
            graph.Couple(game, actionUse).Classify("action");
            graph.Couple(game, actionRead).Classify("action");
            graph.Couple(game, actionTake).Classify("action");
            graph.Couple(game, actionDrop).Classify("action");
            graph.Couple(game, actionOpen).Classify("action");
            graph.Couple(game, actionClose).Classify("action");

            var field = (Node)graph.Add()
                .Classify("location")
                .Classify("spawnpoint")
                .Qualify("name", "field")
                .Qualify("description", "empty field");
            graph.Couple(map, field).Classify("spawnpoint");
            graph.Couple(field, actionLook).Classify("action");
            graph.Couple(field, actionGo).Classify("action");

            var castle = (Node)graph.Add()
                .Classify("location")
                .Qualify("name", "castle")
                .Qualify("description", "crumbling castle");
            graph.Couple(map, castle);
            graph.Couple(castle, actionLook).Classify("action");
            graph.Couple(castle, actionGo).Classify("action");

            var mountains = (Node)graph.Add()
                .Classify("location")
                .Qualify("name", "mountains")
                .Qualify("description", "snow covered mountains");
            graph.Couple(map, mountains);
            graph.Couple(mountains, actionLook).Classify("action");
            graph.Couple(mountains, actionGo).Classify("action");

            var village = (Node)graph.Add()
                .Classify("location")
                .Qualify("name", "village")
                .Qualify("description", "a small peasant village");
            graph.Couple(map, village);
            graph.Couple(village, actionLook).Classify("action");
            graph.Couple(village, actionGo).Classify("action");

            graph.Couple(field, castle)
                .Classify("path")
                .Qualify("length", "10");

            graph.Couple(field, mountains)
                .Classify("path")
                .Qualify("length", "10");

            graph.Couple(field, village)
                .Classify("path")
                .Qualify("length", "10");

            graph.Couple(mountains, castle)
                .Classify("path")
                .Qualify("length", "10");

            graph.Couple(village, castle)
                .Classify("path")
                .Qualify("length", "10");

            var sword = (Node)graph.Add()
                .Classify("object")
                .Qualify("name", "rusty sword")
                .Qualify("description", "a sword with rust on it");
            graph.Couple(sword, actionLook).Classify("action");
            graph.Couple(sword, actionTake).Classify("action");
            graph.Couple(sword, actionUse).Classify("action");
            graph.Couple(sword, actionDrop).Classify("action");

            var chest = (Node)graph.Add()
                .Classify("object")
                .Qualify("name", "wooden chest")
                .Qualify("description", "a chest made of wood");
            graph.Couple(chest, actionLook).Classify("action");
            graph.Couple(chest, actionOpen).Classify("action");
            graph.Couple(chest, actionClose).Classify("action");

            var scroll = (Node)graph.Add()
                .Classify("object")
                .Qualify("name", "magic scroll")
                .Qualify("description", "an ancient scroll containing magic spell");
            graph.Couple(scroll, actionLook).Classify("action");
            graph.Couple(scroll, actionTake).Classify("action");
            graph.Couple(scroll, actionRead).Classify("action");
            graph.Couple(scroll, actionDrop).Classify("action");

            graph.Couple(player, sword)
                .Classify("inventory");

            graph.Couple(player, field)
                .Classify("current");

            graph.Couple(castle, chest)
                .Classify("contains");

            graph.Couple(chest, scroll)
                .Classify("contains");

            var uprisingEvent = (Node)graph.Add()
                .Classify("event")
                .Qualify("name", "peasant uprising")
                .Qualify("chance", "70")
                .Qualify("result", "death");

            var magicEvent = (Node)graph.Add()
                .Classify("event")
                .Qualify("name", "witch discovery")
                .Qualify("chance", "80")
                .Qualify("result", "death");

            var trollEvent = (Node)graph.Add()
                .Classify("event")
                .Qualify("name", "evil mountain troll")
                .Qualify("chance", "90")
                .Qualify("result", "death");

            graph.Couple(village, uprisingEvent)
                .Classify("event");

            graph.Couple(castle, magicEvent)
                .Classify("event");

            graph.Couple(mountains, trollEvent)
                .Classify("event");

            var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var repository = new JsonRepository<Graph>(System.IO.Path.Combine(myDocs, $"{gameName}.{DateTime.Now.ToFileTime()}"));
            _ = repository.Insert(graph);
        }
    }
}
