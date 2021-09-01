using Xunit;

namespace Graphs.Documents.Tests
{ 
    public class DocumentTests
    {
        [Fact]
        public void KeyInfoCache_IsFilled()
        {
            Assert.Equal(nameof(Member), DocumentKeys<Member>.TypeName);
            Assert.Single(DocumentKeys<Member>.KeyProperties);
            Assert.Contains(DocumentKeys<Member>.KeyProperties, p => p.Name == nameof(Member.Id));
        }

        [Fact]
        public void Document_ExplicitOperatorT_ReturnsDocumentT()
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

        [Fact]
        public void Document_ExplicitOperatorDocumentT_ReturnsT()
        {
            var member1 = new Member();
            var document = (Document<Member>)member1;
            Assert.NotNull(document);
            Assert.NotNull(document.Member);
            Assert.Equal(typeof(Member), document.Member.GetType());
            Assert.Equal(member1.Id.ToString(), document.Key);

            var member2 = (Member)document;
            Assert.Equal(member1.Id, member2.Id);
        }

        [Fact]
        public void Document_Clone_Updates_ETag()
        {
            var member = new Member();
            var document = (Document<Member>)member;
            Assert.NotNull(document);
            Assert.NotNull(document.Member);
            Assert.Equal(typeof(Member), document.Member.GetType());
            Assert.Equal(member.Id.ToString(), document.Key);

            var clone = (Document<Member>)document.Clone();
            Assert.NotEqual(document.ETag, clone.ETag);
        }

        [Fact]
        public void Document_EventArgs()
        {
            var document = (Document<Member>)new Member();

            var documentEventArgs = new DocumentEventArgs<Member>(document);
            Assert.Equal(document, documentEventArgs.Document);

            var documentAddedEventArgs = new DocumentAddedEventArgs<Member>(document);
            Assert.Equal(document, documentAddedEventArgs.Document);

            var documentUpdatedEventArgs = new DocumentUpdatedEventArgs<Member>(document);
            Assert.Equal(document, documentUpdatedEventArgs.Document);

            var documentRemovedEventArgs = new DocumentRemovedEventArgs<Member>(document);
            Assert.Equal(document, documentRemovedEventArgs.Document);
        }
    }
}
