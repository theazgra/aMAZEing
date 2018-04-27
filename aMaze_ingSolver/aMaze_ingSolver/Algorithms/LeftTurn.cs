using aMaze_ingSolver.GraphUtils;
using System.Collections.Generic;

namespace aMaze_ingSolver.Algorithms
{
    class LeftTurn : MazeSolver
    {
        public override bool SupportParallel => false;
        public override string Name => "Left turn";

        public override event Solved OnSolved;

        public override void SolveMaze(Graph graph)
        {
            _timer.Start();
            _resultPath = new Queue<Vertex>();

            _resultPath.Enqueue(graph.Start);

            Vertex current = graph.Start.GetNeighbour(Direction.Down);
            Vertex previous = graph.Start;

            Direction currentHeading = Direction.Down;
            int turn = 1;

            while (true)
            {
                _resultPath.Enqueue(current);

                if (current.Equals(graph.End))
                {
                    _timer.Stop();
                    OnSolved?.Invoke();
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

    }
}
