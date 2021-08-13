using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Indexes
{
    public abstract class IncidenceMatrix
        : GraphIndex<int>
    {


        private int size;
        public override int Size => this.size;
    }
}
