using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMaze_ingSolver.GraphUtils
{
    class VertexPair
    {
        public Vertex Current { get; }
        public Vertex Next { get; }

        /// <summary>
        /// current 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="next"></param>
        public VertexPair(Vertex current, Vertex next)
        {
            Current = current;
            Next = next;
        }
    }
}
