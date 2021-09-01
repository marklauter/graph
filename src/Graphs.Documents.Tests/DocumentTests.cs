using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Graphs.Documents.Tests
{
    public sealed class KeylessMember
    {

    }

    public sealed class Member
    {
        [Key]
        public Guid Id { get; } = Guid.NewGuid();
    }

    public class DocumentTests
    {
        [Fact]
        public void KeyInfoCache_IsFilled()
        {
            Assert.Equal(nameof(Member), KeyInfoCache<Member>.TypeName);
            Assert.Single(KeyInfoCache<Member>.KeyProperties);
            Assert.Contains(KeyInfoCache<Member>.KeyProperties, p => p.Name == nameof(Member.Id));
        }

        [Fact]
        public void DocumentExplicitOperatorT_ReturnsDocumentT()
        {
            var member = new Member();
            var document1 = (Document<Member>)member;
            Assert.NotNull(document1);
            Assert.NotNull(document1.Member);
            Assert.Equal(typeof(Member), document1.Member.GetType());
            Assert.Equal(member.Id.ToString(), document1.Key);

            var keylessMember = new KeylessMember();
            var document2 = (Document<KeylessMember>)keylessMember;
            Assert.NotNull(document2);
            Assert.NotNull(document2.Member);
            Assert.Equal(typeof(KeylessMember), document2.Member.GetType());
            Assert.Equal(keylessMember.GetHashCode().ToString(), document2.Key);
        }

        public void DocumentExplicitOperatorDocumentT_ReturnsDocumentT()
        {
            var member = new Member();
            var document = (Document<Member>)member;
            Assert.NotNull(document);
            Assert.NotNull(document.Member);
            Assert.Equal(typeof(Member), document.Member.GetType());

        }
    }
}
