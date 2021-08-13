namespace Graph.Indexes.Tests
{
    public abstract class AdjacencyIndexTests
    {
        protected abstract IGraphIndex<int> EmptyIndex();
        public abstract void AdjacencyIndex_Clone_Succeeds();
        public abstract void AdjacencyIndex_Couple_Succeeds();
        public abstract void AdjacencyIndex_Degrees_Succeeds();
        public abstract void AdjacencyIndex_Disconnect_Succeeds();
        public abstract void AdjacencyIndex_Neighbors_Succeeds();
    }
}
