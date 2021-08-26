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

            var inventory = (Node)graph.Add()
                .Classify("inventory")
                .Classify("container")
                .Qualify("maxWeight", "20");
            
            var map = (Node)graph.Add()
                .Classify("map");

            _ = graph.Couple(game, player);
            _ = graph.Couple(game, map);
            _ = graph.Couple(player, inventory);

            // todo: consider converting handlers from action attributes to nodes on the graph - attributes could define the arguments and return types use the transferHandler and actionGo as a template
            var transferHandler = (Node)graph.Add()
                .Classify("handler")
                .Qualify("classname", "MoveActionHandler"); // transfer the player from origin location to destination location

            var describeHandler = (Node)graph.Add()
                .Classify("handler")
                .Qualify("classname", "DescribeActionHandler"); // transfer the player from origin location to destination location

            var actionGo = (Node)graph.Add()
                .Classify("action")
                .Qualify("name", "go");

            _ = graph.Couple(actionGo, transferHandler);

            var lookAction = (Node)graph.Add()
                .Classify("action")
                .Qualify("name", "look")
                .Qualify("handler", "describe"); // displays detailed description to player

            var examineAction = (Node)graph.Add()
                .Classify("action")
                .Qualify("name", "examine")
                .Qualify("handler", "describe"); // displays detailed description to player

            _ = graph.Couple(lookAction, describeHandler);
            _ = graph.Couple(examineAction, describeHandler);

            var actionUse = (Node)graph.Add()
                .Classify("action")
                .Qualify("name", "use")
                .Qualify("handler", "use"); // applies object consequences to target - for example, use sword on ork will result in an attempt to hit an ork with the sword, so a roll for hit chance and then a roll for damage

            var readAction = (Node)graph.Add()
                .Classify("action")
                .Qualify("name", "read")
                .Qualify("handler", "describe"); // displays detailed description to player

            var takeAction = (Node)graph.Add()
                .Classify("action")
                .Qualify("name", "take")
                .Qualify("handler", "transfer"); // transfers object from origin location to player

            var dropAction = (Node)graph.Add()
                .Classify("action")
                .Qualify("name", "drop")
                .Qualify("handler", "transfer"); // transfers object from player to target location

            var openAction = (Node)graph.Add()
                .Classify("action")
                .Qualify("name", "open")
                .Qualify("handler", "open"); // changes state of container to open, makes contents accessible

            var closeAction = (Node)graph.Add()
                .Classify("action")
                .Qualify("name", "close")
                .Qualify("handler", "close"); // changes state of container to closed, makes contents inaccessible

            _ = graph.Couple(game, actionGo)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(game, lookAction)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(game, examineAction)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(game, actionUse)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(game, readAction)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(game, takeAction)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(game, dropAction)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(game, openAction)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(game, closeAction)
                .Classify("vocabulary")
                .Classify("verb");

            _ = graph.Couple(inventory, lookAction).Classify("action");
            _ = graph.Couple(inventory, examineAction).Classify("action");

            var field = (Node)graph.Add()
                .Classify("location")
                .Classify("spawnpoint")
                .Qualify("name", "field")
                .Qualify("description", "empty field");
            
            _ = graph.Couple(map, field).Classify("spawnpoint");
            _ = graph.Couple(field, lookAction).Classify("action");
            _ = graph.Couple(field, examineAction).Classify("action");
            _ = graph.Couple(field, actionGo).Classify("action");

            var castle = (Node)graph.Add()
                .Classify("location")
                .Qualify("name", "castle")
                .Qualify("description", "crumbling castle. might be haunted.");
            
            _ = graph.Couple(map, castle);
            _ = graph.Couple(castle, lookAction).Classify("action");
            _ = graph.Couple(castle, examineAction).Classify("action");
            _ = graph.Couple(castle, actionGo).Classify("action");

            var mountains = (Node)graph.Add()
                .Classify("location")
                .Qualify("name", "mountains")
                .Qualify("description", "snow covered mountains. idea habitat for mountain trolls.");
            
            _ = graph.Couple(map, mountains);
            _ = graph.Couple(mountains, lookAction).Classify("action");
            _ = graph.Couple(mountains, examineAction).Classify("action");
            _ = graph.Couple(mountains, actionGo).Classify("action");

            var village = (Node)graph.Add()
                .Classify("location")
                .Qualify("name", "village")
                .Qualify("description", "a small peasant village. probably filled with friendly, helpful people.");
            
            _ = graph.Couple(map, village);
            _ = graph.Couple(village, lookAction).Classify("action");
            _ = graph.Couple(village, examineAction).Classify("action");
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
                .Qualify("description", "a sword. with rust.");
            
            _ = graph.Couple(sword, lookAction).Classify("action");
            _ = graph.Couple(sword, examineAction).Classify("action");
            _ = graph.Couple(sword, takeAction).Classify("action");
            _ = graph.Couple(sword, actionUse).Classify("action");
            _ = graph.Couple(sword, dropAction).Classify("action");

            var chest = (Node)graph.Add()
                .Classify("object")
                .Classify("container")
                .Qualify("name", "wooden chest")
                .Qualify("description", "a chest made of wood. it's closed.");
            
            _ = graph.Couple(chest, lookAction).Classify("action");
            _ = graph.Couple(chest, examineAction).Classify("action");
            _ = graph.Couple(chest, openAction).Classify("action");
            _ = graph.Couple(chest, closeAction).Classify("action");

            var scroll = (Node)graph.Add()
                .Classify("object")
                .Qualify("name", "magic scroll")
                .Qualify("description", "an ancient scroll. maybe it contains magic spells.");
            
            _ = graph.Couple(scroll, lookAction).Classify("action");
            _ = graph.Couple(scroll, examineAction).Classify("action");
            _ = graph.Couple(scroll, takeAction).Classify("action");
            _ = graph.Couple(scroll, readAction).Classify("action");
            _ = graph.Couple(scroll, dropAction).Classify("action");

            _ = graph.Couple(inventory, sword).Classify("contains");
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
