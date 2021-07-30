namespace Graph.Sets
{
    public class EdgeMatrix
    {
        private readonly bool[,] matrix;

        public static EdgeMatrix Empty { get; } = new();

        public static EdgeMatrix Build(Graph graph)
        {
            return new EdgeMatrix(graph);
        }

        private EdgeMatrix()
        {
            this.matrix = new bool[0, 0];
        }

        private EdgeMatrix(Graph graph)
        {
            this.matrix = new bool[graph.Size, graph.Size];
            for (var o = graph.Size - 1; o != 0; --o)
            {
                for (var i = graph.Size - 1; i != 0; --i)
                {
                    this.matrix[o, i] = false;
                }
            }
        }
    }
}
