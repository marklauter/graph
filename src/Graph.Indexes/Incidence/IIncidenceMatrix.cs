using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Indexes.Incidence
{
    public interface IIncidenceMatrix<TKey>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        IEnumerable<TKey> Edges(TKey node);
    }
}
