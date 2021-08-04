using Graph.DB.Elements;
using Graph.DB.IO;
using Graph.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.DB
{
    public class RepositoryGraph
        : IGraph<Vertex>
    {
        private readonly IRepository<Vertex> vertexRepository;
        private readonly IRepository<Edge> edgeRepository;

        public RepositoryGraph(
            IRepository<Vertex> vertexRepository,
            IRepository<Edge> edgeRepository)
        {
            if (vertexRepository is null)
            {
                throw new ArgumentNullException(nameof(vertexRepository));
            }

            if (edgeRepository is null)
            {
                throw new ArgumentNullException(nameof(edgeRepository));
            }

            this.vertexRepository = vertexRepository;
            this.edgeRepository = edgeRepository;
        }

        public int Size { get; }

        public GraphType Type { get; }

        public int Add(Vertex vertex)
        {
            vertexRepository.Insert(vertex);
        }

        public void AddRange(IEnumerable<Vertex> vertices)
        {
            throw new NotImplementedException();
        }

        public bool Adjacent(Vertex vertex1, Vertex vertex2)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Vertex> BreadthFirstSearch(Vertex vertex)
        {
            throw new NotImplementedException();
        }

        public IGraph<Vertex> Clone()
        {
            throw new NotImplementedException();
        }

        public void Connect(Vertex vertex1, Vertex vertex2)
        {
            throw new NotImplementedException();
        }

        public int Degree(Vertex vertex)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Vertex> DepthFirstSearchPostOrder(Vertex vertex)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Vertex> DepthFirstSearchPreOrder(Vertex vertex)
        {
            throw new NotImplementedException();
        }

        public void Disconnect(Vertex vertex1, Vertex vertex2)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Vertex> Neighbors(Vertex vertex)
        {
            throw new NotImplementedException();
        }
    }
}
