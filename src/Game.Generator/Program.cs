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

            _ = graph.Couple(game, player);
            _ = graph.Couple(game, map);

            var actionGo = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "go")
                .Qualify("handler", "transfer"); // transfer the player from origin location to destination location

            var actionLook = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "look")
                .Qualify("handler", "describe"); // displays detailed description to player

            var actionUse = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "use")
                .Qualify("handler", "use"); // applies object consequences to target - for example, use sword on ork will result in an attempt to hit an ork with the sword, so a roll for hit chance and then a roll for damage

            var actionRead = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "read")
                .Qualify("handler", "describe"); // displays detailed description to player

            var actionTake = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "take")
                .Qualify("handler", "transfer"); // transfers object from origin location to player

            var actionDrop = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "drop")
                .Qualify("handler", "transfer"); // transfers object from player to target location

            var actionOpen = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "open")
                .Qualify("handler", "open"); // changes state of container to open, makes contents accessible

            var actionClose = (Node)graph.Add()
                .Classify("action")
                .Qualify("value", "close")
                .Qualify("handler", "close"); // changes state of container to closed, makes contents inaccessible

            _ = graph.Couple(game, actionGo)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(game, actionLook)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(game, actionUse)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(game, actionRead)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(game, actionTake)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(game, actionDrop)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(game, actionOpen)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(game, actionClose)
                .Classify("vocabulary")
                .Classify("verb");

            var field = (Node)graph.Add()
                .Classify("location")
                .Classify("spawnpoint")
                .Qualify("name", "field")
                .Qualify("description", "empty field");
            
            _ = graph.Couple(map, field).Classify("spawnpoint");
            _ = graph.Couple(field, actionLook).Classify("action");
            _ = graph.Couple(field, actionGo).Classify("action");

            var castle = (Node)graph.Add()
                .Classify("location")
                .Qualify("name", "castle")
                .Qualify("description", "crumbling castle");
            
            _ = graph.Couple(map, castle);
            _ = graph.Couple(castle, actionLook).Classify("action");
            _ = graph.Couple(castle, actionGo).Classify("action");

            var mountains = (Node)graph.Add()
                .Classify("location")
                .Qualify("name", "mountains")
                .Qualify("description", "snow covered mountains");
            
            _ = graph.Couple(map, mountains);
            _ = graph.Couple(mountains, actionLook).Classify("action");
            _ = graph.Couple(mountains, actionGo).Classify("action");

            var village = (Node)graph.Add()
                .Classify("location")
                .Qualify("name", "village")
                .Qualify("description", "a small peasant village");
            
            _ = graph.Couple(map, village);
            _ = graph.Couple(village, actionLook).Classify("action");
            _ = graph.Couple(village, actionGo).Classify("action");

            _ = graph.Couple(field, castle)
                .Classify("path")
                .Qualify("length", "20");

            _ = graph.Couple(field, mountains)
                .Classify("path")
                .Qualify("length", "20");

            _ = graph.Couple(field, village)
                .Classify("path")
                .Qualify("length", "10");

            _ = graph.Couple(mountains, castle)
                .Classify("path")
                .Qualify("length", "28");

            _ = graph.Couple(village, castle)
                .Classify("path")
                .Qualify("length", "22");

            var sword = (Node)graph.Add()
                .Classify("object")
                .Qualify("name", "rusty sword")
                .Qualify("description", "a sword with rust on it");
            
            _ = graph.Couple(sword, actionLook).Classify("action");
            _ = graph.Couple(sword, actionTake).Classify("action");
            _ = graph.Couple(sword, actionUse).Classify("action");
            _ = graph.Couple(sword, actionDrop).Classify("action");

            var chest = (Node)graph.Add()
                .Classify("object")
                .Qualify("name", "wooden chest")
                .Qualify("description", "a chest made of wood");
            
            _ = graph.Couple(chest, actionLook).Classify("action");
            _ = graph.Couple(chest, actionOpen).Classify("action");
            _ = graph.Couple(chest, actionClose).Classify("action");

            var scroll = (Node)graph.Add()
                .Classify("object")
                .Qualify("name", "magic scroll")
                .Qualify("description", "an ancient scroll containing magic spell");
            
            _ = graph.Couple(scroll, actionLook).Classify("action");
            _ = graph.Couple(scroll, actionTake).Classify("action");
            _ = graph.Couple(scroll, actionRead).Classify("action");
            _ = graph.Couple(scroll, actionDrop).Classify("action");

            _ = graph.Couple(player, sword).Classify("inventory");
            _ = graph.Couple(player, field).Classify("current");

            _ = graph.Couple(castle, chest).Classify("contains");
            _ = graph.Couple(chest, scroll).Classify("contains");

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

            _ = graph.Couple(village, uprisingEvent).Classify("event");
            _ = graph.Couple(castle, magicEvent).Classify("event");
            _ = graph.Couple(mountains, trollEvent).Classify("event");

            var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var repository = new JsonRepository<Graph>(System.IO.Path.Combine(myDocs, $"{gameName}.{DateTime.Now.ToFileTime()}"));
            
            _ = repository.Insert(graph);
        }
    }
}
