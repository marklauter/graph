using Xunit;

namespace Graphs.Indexes.Tests
{
    public abstract class DirectedAdjacencyIndexTests
        : AdjacencyIndexTests
    {
        [Fact]
        public override void AdjacencyIndex_Clone_Succeeds()
        {
            var index = this.EmptyIndex();
            index.Couple(0, 1);
            index.Couple(1, 2);

            Assert.False(index.Adjacent(0, 0));
            Assert.True(index.Adjacent(0, 1));
            Assert.False(index.Adjacent(0, 2));

            Assert.False(index.Adjacent(1, 0));
            Assert.False(index.Adjacent(1, 1));
            Assert.True(index.Adjacent(1, 2));

            Assert.False(index.Adjacent(2, 0));
            Assert.False(index.Adjacent(2, 1));
            Assert.False(index.Adjacent(2, 2));

            var clone = (IAdjacencyIndex<int>)index.Clone();

            Assert.False(clone.Adjacent(0, 0));
            Assert.True(clone.Adjacent(0, 1));
            Assert.False(clone.Adjacent(0, 2));

            Assert.False(clone.Adjacent(1, 0));
            Assert.False(clone.Adjacent(1, 1));
            Assert.True(clone.Adjacent(1, 2));

            Assert.False(clone.Adjacent(2, 0));
            Assert.False(clone.Adjacent(2, 1));
            Assert.False(clone.Adjacent(2, 2));
        }

        [Fact]
        public override void AdjacencyIndex_Couple_Succeeds()
        {
            var index = this.EmptyIndex();
            Assert.True(index.Couple(0, 1));

            Assert.False(index.Adjacent(0, 0));
            Assert.True(index.Adjacent(0, 1));
            Assert.False(index.Adjacent(1, 0));
            Assert.False(index.Adjacent(1, 1));

            Assert.False(index.Couple(0, 1));
            Assert.Single(index.Neighbors(0));
        }

        [Fact]
        public override void AdjacencyIndex_Degrees_Succeeds()
        {
            var index = this.EmptyIndex();
            index.Couple(0, 1);
            index.Couple(1, 2);

            Assert.Equal(1, index.Degree(0));
            Assert.Equal(1, index.Degree(1));
            Assert.Equal(0, index.Degree(2));
        }

        [Fact]
        public override void AdjacencyIndex_Disconnect_Succeeds()
        {
            var index = this.EmptyIndex();
            Assert.True(index.Couple(0, 1));

            Assert.True(index.Adjacent(0, 1));
            Assert.False(index.Adjacent(1, 0));

            Assert.True(index.Decouple(0, 1));
            Assert.False(index.Adjacent(0, 1));
            Assert.False(index.Adjacent(1, 0));
            Assert.False(index.Decouple(0, 1));

            Assert.True(index.Couple(0, 1));
            Assert.False(index.Decouple(1, 0));

            Assert.True(index.Adjacent(0, 1));
            Assert.False(index.Adjacent(1, 0));
        }

        [Fact]
        public override void AdjacencyIndex_Neighbors_Succeeds()
        {
            var index = this.EmptyIndex();
            index.Couple(0, 1);
            index.Couple(1, 2);

            var neighbors = index.Neighbors(1);
            Assert.DoesNotContain(0, neighbors);
            Assert.Contains(2, neighbors);
        }
    }
}
