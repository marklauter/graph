using Graphs.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs.Elements
{
    // uses BFS to query the graph - returns frontiers of nodes
    public static class GraphQueries
    {
        public static bool TryGetElement<T>(
            this IGraph source,
            Guid id,
            out T element)
            where T : IElement
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            try
            {
                if (typeof(T) == typeof(Node))
                {
                    element = (T)(IElement)source.Node(id);
                    return true;
                }
                else if (typeof(T) == typeof(Edge))
                {
                    element = (T)(IElement)source.Edge(id);
                    return true;
                }
            }
            catch
            {
                element = default;
                return false;
            }

            throw new InvalidOperationException();
        }

        public static Node FirstOrDefault(
            this IGraph source,
            Func<Node, bool> predicate)
        {
            return source.Nodes.FirstOrDefault(predicate);
        }

        public static IEnumerable<(Node node, int level)> Where<T>(
            this IGraph source,
            Node origin,
            int depth,
            Func<T, bool> predicate)
            where T : IElement
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (origin is null)
            {
                throw new ArgumentNullException(nameof(origin));
            }

            if (depth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(depth));
            }

            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            switch (predicate)
            {
                case Func<Node, bool>:
                    return SearchNodes(source, origin, depth, predicate as Func<Node, bool>);
                case Func<Edge, bool>:
                    return SearchEdges(source, origin, depth, predicate as Func<Edge, bool>);
                default:
                    throw new InvalidOperationException();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4456:Parameter validation in yielding methods should be wrapped", Justification = "shhh")]
        public static IEnumerable<(Node node, int level)> Where(
            this IGraph source,
            Node origin,
            int depth,
            Func<(Edge edge, NodeTypes nodeType), bool> predicate)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (origin is null)
            {
                throw new ArgumentNullException(nameof(origin));
            }

            if (depth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(depth));
            }

            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var visited = new HashSet<Node>(new[] { origin });

            var edges = source.IncidentEdges(origin)
                .Where(predicate)
                .ToArray();

            var sourceIds = edges
                .Where(e => (e.nodeType & NodeTypes.Source) == NodeTypes.Source)
                .Select(e => e.edge.Source);

            var targetIds = edges
                .Where(e => (e.nodeType & NodeTypes.Target) == NodeTypes.Target)
                .Select(e => e.edge.Target);

            var nodeIds = sourceIds
                .Union(targetIds)
                .Distinct()
                .ToHashSet();

            var frontier = source.Neighbors(origin)
                .Where(n => nodeIds.Contains(n.Id))
                .ToArray();

            var level = 1;
            while (frontier.Any() && level <= depth)
            {
                for (var i = 0; i < frontier.Length; ++i)
                {
                    var node = frontier[i];
                    yield return (node, level);
                    visited.Add(node);
                }

                ++level;

                edges = frontier.SelectMany(node => source.IncidentEdges(node))
                    .Where(predicate)
                    .ToArray();

                sourceIds = edges
                    .Where(e => (e.nodeType & NodeTypes.Source) == NodeTypes.Source)
                    .Select(e => e.edge.Source);

                targetIds = edges
                    .Where(e => (e.nodeType & NodeTypes.Target) == NodeTypes.Target)
                    .Select(e => e.edge.Target);

                nodeIds = sourceIds
                    .Union(targetIds)
                    .Distinct()
                    .ToHashSet();

                frontier = frontier
                    .SelectMany(node => source.Neighbors(node))
                    .Where(node => !visited.Contains(node))
                    .Where(n => nodeIds.Contains(n.Id))
                    .ToArray();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4456:Parameter validation in yielding methods should be wrapped", Justification = "shhh")]
        public static IEnumerable<(Node node, int level)> Where(
            this IGraph source,
            Node origin,
            int depth,
            Func<Node, bool> nodePredicate,
            Func<Edge, bool> edgePredicate)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (origin is null)
            {
                throw new ArgumentNullException(nameof(origin));
            }

            if (depth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(depth));
            }

            if (nodePredicate is null)
            {
                throw new ArgumentNullException(nameof(nodePredicate));
            }

            if (edgePredicate is null)
            {
                throw new ArgumentNullException(nameof(edgePredicate));
            }

            var visited = new HashSet<Node>(new[] { origin });

            var edges = source.IncidentEdges(origin)
                .Select(e => e.edge)
                .Where(edgePredicate)
                .ToArray();

            var nodeIds = edges.Select(e => e.Source) // todo: could use select many since edge is enumerable
                .Union(edges.Select(e => e.Target))
                .Distinct()
                .ToHashSet();

            var frontier = source.Neighbors(origin)
                .Where(n => nodeIds.Contains(n.Id))
                .Where(nodePredicate)
                .ToArray();

            var level = 1;
            while (frontier.Any() && level <= depth)
            {
                for (var i = 0; i < frontier.Length; ++i)
                {
                    var node = frontier[i];
                    yield return (node, level);
                    visited.Add(node);
                }

                ++level;

                edges = frontier.SelectMany(node => source.IncidentEdges(node))
                    .Select(e => e.edge)
                    .Where(edgePredicate)
                    .ToArray();

                nodeIds = edges.Select(e => e.Source)
                    .Union(edges.Select(e => e.Target))
                    .Distinct()
                    .ToHashSet();

                frontier = frontier
                    .SelectMany(node => source.Neighbors(node))
                    .Where(node => !visited.Contains(node))
                    .Where(n => nodeIds.Contains(n.Id))
                    .Where(nodePredicate)
                    .ToArray();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4456:Parameter validation in yielding methods should be wrapped", Justification = "shhh")]
        public static IEnumerable<(Node node, int level)> Where(
            this IGraph source,
            Node origin,
            int depth,
            Func<Node, bool> nodePredicate,
            Func<(Edge edge, NodeTypes nodeType), bool> edgePredicate)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (origin is null)
            {
                throw new ArgumentNullException(nameof(origin));
            }

            if (depth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(depth));
            }

            if (nodePredicate is null)
            {
                throw new ArgumentNullException(nameof(nodePredicate));
            }

            if (edgePredicate is null)
            {
                throw new ArgumentNullException(nameof(edgePredicate));
            }

            var visited = new HashSet<Node>(new[] { origin });

            var edges = source.IncidentEdges(origin)
                .Where(edgePredicate)
                .ToArray();

            var sourceIds = edges
                .Where(e => (e.nodeType & NodeTypes.Source) == NodeTypes.Source)
                .Select(e => e.edge.Source);

            var targetIds = edges
                .Where(e => (e.nodeType & NodeTypes.Target) == NodeTypes.Target)
                .Select(e => e.edge.Target);

            var nodeIds = sourceIds
                .Union(targetIds)
                .Distinct()
                .ToHashSet();

            var frontier = source.Neighbors(origin)
                .Where(n => nodeIds.Contains(n.Id))
                .Where(nodePredicate)
                .ToArray();

            var level = 1;
            while (frontier.Any() && level <= depth)
            {
                for (var i = 0; i < frontier.Length; ++i)
                {
                    var node = frontier[i];
                    yield return (node, level);
                    visited.Add(node);
                }

                ++level;

                edges = frontier.SelectMany(node => source.IncidentEdges(node))
                    .Where(edgePredicate)
                    .ToArray();

                sourceIds = edges
                    .Where(e => (e.nodeType & NodeTypes.Source) == NodeTypes.Source)
                    .Select(e => e.edge.Source);

                targetIds = edges
                    .Where(e => (e.nodeType & NodeTypes.Target) == NodeTypes.Target)
                    .Select(e => e.edge.Target);

                nodeIds = sourceIds
                    .Union(targetIds)
                    .Distinct()
                    .ToHashSet();

                frontier = frontier
                    .SelectMany(node => source.Neighbors(node))
                    .Where(node => !visited.Contains(node))
                    .Where(n => nodeIds.Contains(n.Id))
                    .Where(nodePredicate)
                    .ToArray();
            }
        }

        private static IEnumerable<(Node node, int level)> SearchNodes(
            IGraph source,
            Node origin,
            int depth,
            Func<Node, bool> predicate)
        {
            var visited = new HashSet<Node>(new[] { origin });

            var frontier = source.Neighbors(origin)
                .Where(predicate)
                .ToArray();

            var level = 1;
            while (frontier.Any() && level <= depth)
            {
                for (var i = 0; i < frontier.Length; ++i)
                {
                    var node = frontier[i];
                    yield return (node, level);
                    visited.Add(node);
                }

                ++level;
                frontier = frontier
                    .SelectMany(node => source.Neighbors(node))
                    .Where(node => !visited.Contains(node))
                    .Where(predicate)
                    .ToArray();
            }
        }

        private static IEnumerable<(Node node, int level)> SearchEdges(
            IGraph source,
            Node origin,
            int depth,
            Func<Edge, bool> predicate)
        {
            var visited = new HashSet<Node>(new[] { origin });

            var edges = source.IncidentEdges(origin)
                .Select(e => e.edge)
                .Where(predicate)
                .ToArray();

            var nodeIds = edges.Select(e => e.Source)
                .Union(edges.Select(e => e.Target))
                .Distinct()
                .ToHashSet();

            var frontier = source.Neighbors(origin)
                .Where(n => nodeIds.Contains(n.Id))
                .ToArray();

            var level = 1;
            while (frontier.Any() && level <= depth)
            {
                for (var i = 0; i < frontier.Length; ++i)
                {
                    var node = frontier[i];
                    yield return (node, level);
                    visited.Add(node);
                }

                ++level;

                edges = frontier.SelectMany(node => source.IncidentEdges(node))
                    .Select(e => e.edge)
                    .Where(predicate)
                    .ToArray();

                nodeIds = edges.Select(e => e.Source)
                    .Union(edges.Select(e => e.Target))
                    .Distinct()
                    .ToHashSet();

                frontier = frontier
                    .SelectMany(node => source.Neighbors(node))
                    .Where(node => !visited.Contains(node))
                    .Where(n => nodeIds.Contains(n.Id))
                    .ToArray();
            }
        }
    }
}
