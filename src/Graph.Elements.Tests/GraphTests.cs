using Graphs.Indexes;
using System;
using System.Linq;
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
                .Qualify("author", "Alan Cooper");

            var doetBook = (Node)graph.Add()
                .Classify("book")
                .Qualify("title", "Design of Everyday Things")
                .Qualify("author", "Donald Norman");

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

            var trooperBook1 = graph.Couple(trooper, dddBook)
                .Classify("owns");

            var trooperBook2 = graph.Couple(trooper, doetBook)
                .Classify("owns");

            var jimmyBook = graph.Couple(jimmy, dddBook)
                .Classify("owns");

            var marksFriends = graph
                .Where<Edge>(mark, 1, e => e.Is("friend"))
                .Select(frontier => frontier.node);

            var marksBooks = graph
                .Where(mark, 1, n => n.Is("book"), e => e.Is("owns"))
                .Select(frontier => frontier.node);

            var suggestedBooks = marksFriends
                .SelectMany(friend => graph.Where(friend, 1, n => n.Is("book"), e => e.Is("owns")))
                .Select(frontier => frontier.node)
                .Except(marksBooks);

            var friendSuggestions = graph
                .Where(mark, 2, n => n.Is("person"), e => e.Is("friend"))
                .Select(frontier => frontier.node)
                .Except(marksFriends);

            Assert.Contains(trooper, marksFriends);
            Assert.Contains(dddBook, marksBooks);
            Assert.Contains(afBook, marksBooks);
            Assert.DoesNotContain(doetBook, marksBooks);
            Assert.Contains(doetBook, suggestedBooks);
            Assert.DoesNotContain(dddBook, suggestedBooks);
            Assert.DoesNotContain(afBook, suggestedBooks);
            Assert.Contains(bobby, friendSuggestions);
            Assert.DoesNotContain(trooper, friendSuggestions);
        }
    }
}
