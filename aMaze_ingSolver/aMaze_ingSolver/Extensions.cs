using aMaze_ingSolver.Tree;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMaze_ingSolver
{
    static class Extensions
    {
        public static bool ContainsLocation(this IEnumerable<Node> nodes, Point location)
        {
            Node found = nodes.FirstOrDefault(n => n.X == location.X && n.Y == location.Y);

            return (found != null);
        }
    }
}
