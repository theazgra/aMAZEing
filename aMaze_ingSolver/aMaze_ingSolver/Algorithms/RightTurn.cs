using aMaze_ingSolver.GraphUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMaze_ingSolver.Algorithms
{
    static class RightTurn
    {
        public static Queue<Vertex> SolveMaze(Graph graph)
        {

            Queue<Vertex> path = new Queue<Vertex>();
            
            path.Enqueue(graph.Start);

            Vertex current = graph.Start.GetNeighbour(Direction.Down);
            Vertex previous = graph.Start;

            Direction currentHeading = Direction.Down;

            int turn = 1;

            bool completed = false;

            while(true)
            {
                path.Enqueue(current);

                if(current.Equals(graph.End))
                {
                    completed = true;
                    break;
                }


                if(!current.HasNeighbour((Direction)(Utils.Modulo((int)currentHeading - (int)turn, 4))))
                {

                   int temp = ((int)currentHeading - (int)turn) % 4;
                    currentHeading = (Direction)(Utils.Modulo((int)currentHeading - (int)turn, 4));
                    current = current.GetNeighbour(currentHeading);
                    continue;
                }

                if (!current.HasNeighbour(currentHeading))
                {
                    current = current.GetNeighbour(currentHeading);
                    continue;
                }

                if (!current.HasNeighbour((Direction)(((int)currentHeading + (int)turn) % 4)))
                {
                    currentHeading = (Direction)(((int)currentHeading + (int)turn) % 4);
                    current = current.GetNeighbour(currentHeading);
                    continue;
                }

                if (!current.HasNeighbour((Direction)(((int)currentHeading + 2) % 4)))
                {
                    currentHeading = (Direction)(((int)currentHeading + 2) % 4);
                    current = current.GetNeighbour(currentHeading);
                    continue;
                }

                completed = false;
                return null;

            }

            


            return path;

        }
    }
}
