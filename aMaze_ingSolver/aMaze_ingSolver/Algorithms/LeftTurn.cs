using aMaze_ingSolver.GraphUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMaze_ingSolver.Algorithms
{
    class LeftTurn : IMazeSolver
    {
        private Graph _graph;
        private Stopwatch _solveTimer = new Stopwatch();
        Queue<Vertex> _resultPath;
        public bool Parallel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Name => "Left turn";

        public bool Solved { get; set; }

        public event solved OnSolved;

        public Queue<Vertex> GetResultVertices()
        {
            return _resultPath;
        }

        public TimeSpan GetSolveTime()
        {
            return _solveTimer.Elapsed;
        }

        public void SolveMaze(Graph graph)
        {
            _solveTimer.Start();
            _graph = graph;
            _resultPath = new Queue<Vertex>();

            _resultPath.Enqueue(_graph.Start);

            Vertex current = _graph.Start.GetNeighbour(Direction.Down);
            Vertex previous = _graph.Start;

            Direction currentHeading = Direction.Down;
            int turn = 1;

            while (true)
            {
                _resultPath.Enqueue(current);

                if (current.Equals(_graph.End))
                {
                    _solveTimer.Stop();
                    OnSolved?.Invoke(   );
                    break;
                }

                if (current.HasNeighbour((Direction)(Utils.Modulo((int)currentHeading - (int)turn, 4))))
                {

                    int temp = ((int)currentHeading - (int)turn) % 4;
                    currentHeading = (Direction)(Utils.Modulo((int)currentHeading - (int)turn, 4));
                    current = current.GetNeighbour(currentHeading);
                    continue;
                }

                if (current.HasNeighbour(currentHeading))
                {
                    current = current.GetNeighbour(currentHeading);
                    continue;
                }

                if (current.HasNeighbour((Direction)(((int)currentHeading + (int)turn) % 4)))
                {
                    currentHeading = (Direction)(((int)currentHeading + (int)turn) % 4);
                    current = current.GetNeighbour(currentHeading);
                    continue;
                }

                if (current.HasNeighbour((Direction)(((int)currentHeading + 2) % 4)))
                {
                    currentHeading = (Direction)(((int)currentHeading + 2) % 4);
                    current = current.GetNeighbour(currentHeading);
                    continue;
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }



        //public static Queue<Vertex> SolveMaze(Graph graph)
        //{

        //    Queue<Vertex> path = new Queue<Vertex>();

        //    path.Enqueue(graph.Start);

        //    Vertex current = graph.Start.GetNeighbour(Direction.Down);
        //    Vertex previous = graph.Start;

        //    Direction currentHeading = Direction.Down;

        //    int turn = 1;

        //    bool completed = false;

        //    while(true)
        //    {
        //        path.Enqueue(current);

        //        if(current.Equals(graph.End))
        //        {
        //            completed = true;
        //            break;
        //        }


        //        if(current.HasNeighbour((Direction)(Utils.Modulo((int)currentHeading - (int)turn, 4))))
        //        {

        //           int temp = ((int)currentHeading - (int)turn) % 4;
        //            currentHeading = (Direction)(Utils.Modulo((int)currentHeading - (int)turn, 4));
        //            current = current.GetNeighbour(currentHeading);
        //            continue;
        //        }

        //        if (current.HasNeighbour(currentHeading))
        //        {
        //            current = current.GetNeighbour(currentHeading);
        //            continue;
        //        }

        //        if (current.HasNeighbour((Direction)(((int)currentHeading + (int)turn) % 4)))
        //        {
        //            currentHeading = (Direction)(((int)currentHeading + (int)turn) % 4);
        //            current = current.GetNeighbour(currentHeading);
        //            continue;
        //        }

        //        if (current.HasNeighbour((Direction)(((int)currentHeading + 2) % 4)))
        //        {
        //            currentHeading = (Direction)(((int)currentHeading + 2) % 4);
        //            current = current.GetNeighbour(currentHeading);
        //            continue;
        //        }

        //        completed = false;
        //        return null;

        //    }




        //    return path;

        //}


    }
}
