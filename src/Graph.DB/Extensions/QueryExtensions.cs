//using Graphs.DB.Elements;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Graphs.DB.Extensions
//{
//    public static class QueryExtensions
//    {
//        public static IEnumerable<(Node<TId> node, int level)> Where<TId, T>(
//            this Node<TId> origin,
//            int maxDepth,
//            Func<T, bool> predicate)
//            where TId : IComparable, IComparable<TId>, IEquatable<TId>
//        {
//            switch (predicate)
//            {
//                case Func<Node<TId>, bool>:
//                    return origin.SearchNodes(maxDepth, predicate as Func<Node<TId>, bool>);
//                case Func<Edge<TId>, bool>:
//                    return origin.SearchEdges(maxDepth, predicate as Func<Edge<TId>, bool>);
//            }
//        }

//        private static IEnumerable<(Node<TId> node, int level)> SearchNodes<TId>(
//            this Node<TId> origin,
//            int maxDepth,
//            Func<Node<TId>, bool> predicate)
//            where TId : IComparable, IComparable<TId>, IEquatable<TId>
//        {
//            var visited = new HashSet<Node<TId>>(new[] { origin });

//            // todo: someone has to be responsible for loading nodes from file if they aren't in memory
//            // source has to be graph and the graph will be responsible for providing nodes and edges from the cache
//            // for now it's time to start writing tests and prove the new repository works, then write the new graph, then write the new linq to graph expressions

//            var frontier = origin.Neighbors()
//                .Select()
//                .Where(predicate)
//                .ToArray();
//        }

//        private static IEnumerable<(Node<TId> node, int level)> SearchEdges<TId>(
//            this Node<TId> origin,
//            int maxDepth,
//            Func<Edge<TId>, bool> predicate)
//            where TId : IComparable, IComparable<TId>, IEquatable<TId>
//        {

//        }
//    }
//}
