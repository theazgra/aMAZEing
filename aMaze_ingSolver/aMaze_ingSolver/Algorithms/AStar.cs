using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aMaze_ingSolver.GraphUtils;

namespace aMaze_ingSolver.Algorithms
{
    class AStar : MazeSolver
    {
        public override string Name => "A*";
        public override bool SupportParallel => false;

        public override event Solved OnSolved;
        public override event SolveProgress OnSolveProgress;
        private bool _invokeEvents = false;

        private List<Vertex> _unvisited;


        private void AddNewBestDistance(Vertex previous, Vertex current, float distance)
        {
            current.Previous = previous;
            current.BestPathDistance = distance;

            if (!_unvisited.Contains(current))
            {
                _unvisited.Add(current);
            }
            _unvisited = _unvisited.OrderBy(v => v.TotalPathDistance).ToList();
            //_unvisited = _unvisited.OrderBy(v => v.BestPathDistance).ToList();
        }

        private Vertex GetTopUnvisitedVertex()
        {
            return _unvisited.First();
        }


        public override void SolveMaze(Graph graph)
        {
            _invokeEvents = MazeForm.InvokeDelegates();
            long loopIteration = 0;
            int vertCount = graph.Vertices.Count;
            int invokeStep = (int)Math.Ceiling((float)(vertCount / 2000));
            graph.Reset();
            _resultPath.Clear();

            _timer.Start();
            //TODO: Parallelize this, it should be pretty simple.
            graph.CalculateDistancesFromVerticesToEnd(ThreadCount);

            _unvisited = new List<Vertex>();
            AddNewBestDistance(null, graph.Start, 0);
            Vertex current = null;

            //Start by adding start's neighbours
            foreach (Vertex neighbours in graph.Start.Neighbours)
            {
                AddNewBestDistance(graph.Start, neighbours, float.PositiveInfinity);
            }

            int iteration = 0;
            //loop while end is not visited.
            while (!graph.End.Visited)
            {
                ++iteration;
                current = GetTopUnvisitedVertex();

                foreach (Vertex neighbour in current.Neighbours)
                {
                    if (neighbour.Visited)
                        continue;

                    //distance to connected vertex using best distance to current vertex.
                    float distanceToVertex = current.BestPathDistance + current.PathDistanceTo(neighbour);
                    if (distanceToVertex < neighbour.BestPathDistance)
                    {
                        AddNewBestDistance(current, neighbour, distanceToVertex);
                    }
                }
                current.Visited = true;
                _unvisited.Remove(current);

                ++loopIteration;
                if (_invokeEvents && iteration >= invokeStep)
                {
                    iteration = 0;
                    float perc = ((float)loopIteration / (float)vertCount) * 100.0f;
                    OnSolveProgress?.Invoke(perc);
                }
            }

            current = graph.End;
            _resultPath.Enqueue(current);
            while (current != graph.Start)
            {
                current = current.Previous;
                //current = _pathToVertex[current].Item1;
                _resultPath.Enqueue(current);
            }
            _timer.Stop();
            OnSolved?.Invoke();


            _timer.Stop();
            OnSolved?.Invoke();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
