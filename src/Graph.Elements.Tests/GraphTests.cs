using Graphs.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Graphs.Elements.Tests
{
    public sealed class GraphTests
    {
        [Fact]
        public void GraphQueries_Success()
        {
            var graphLabel = "gaph";
            var version = "0.0";
            var index = UndirectedAdjacencyList<Guid>.Empty();
            var graph = new Graph(index);

            graph.Classify(graphLabel)
                .Qualify("version", version);

            var mark = (Node)graph.Add()
                .Classify("person")
                .Qualify("name", "Mark");

            var dddBook = (Node)graph.Add()
                .Classify("book")
                .Qualify("title", "Domain Driven Design")
                .Qualify("author", "Eric Evans");

            var afBook = (Node)graph.Add()
                .Classify("book")
                .Qualify("title", "About Face")
                .Qualify("author","Alan Cooper") ;

            var trooper = (Node)graph.Add()
                .Classify("person")
                .Qualify("name", "Trooper");

            var jimmy = (Node)graph.Add()
                .Classify("person")
                .Qualify("name", "Jimmy");

            var bobby = (Node)graph.Add()
                .Classify("person")
                .Qualify("name", "Bobby");

            var markFriend = graph.Couple(mark, trooper)
                .Classify("friend")
                .Qualify("since", "1980-01-01T00:00:00");

            var markCoworker = graph.Couple(mark, jimmy)
                .Classify("coworker")
                .Qualify("company", "Contoso");

            var jimmyFriend = graph.Couple(jimmy, trooper)
                .Classify("friend")
                .Qualify("since", "2010-06-12T00:00:00");

            var trooperFriend = graph.Couple(trooper, bobby)
                .Classify("friend")
                .Qualify("since", "2010-06-12T00:00:00");

            var markBook1 = graph.Couple(mark, dddBook)
                .Classify("owns");

            var markBook2 = graph.Couple(mark, afBook)
                .Classify("owns");

            var trooperBook = graph.Couple(trooper, dddBook)
                .Classify("owns");

            var jimmyBook = graph.Couple(jimmy, dddBook)
                .Classify("owns");

            var friends = graph
                .Where<Edge>(mark, 1, e => e.Is("friend"));

            var marksBooks = graph
                .Where(mark, 1, n => n.Is("book"), e => e.Is("owns"));

            var friendSuggestions = graph
                .Where(mark, 2, n => n.Is("person"), e => e.Is("friend"))
                .Except(friends);

            Assert.Contains((trooper, 1), friends);

            Assert.Contains((dddBook, 1), marksBooks);
            Assert.Contains((afBook, 1), marksBooks);

            Assert.Contains((bobby, 2), friendSuggestions);
        }
    }
}
