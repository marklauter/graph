﻿using Graphs.Elements;
using Graphs.IO;
using System;
using System.IO;

namespace Game.Controller.Tests
{
    internal static class GraphFactory
    {
        public static Graph CreateGraph(Guid graphId)
        {
            var repositoryName = Path.Combine("Data", "adventure.132743028908632345");
            var repository = new JsonRepository<Graph>(repositoryName);
            return (Graph)repository.Read(graphId);
        }
    }
}