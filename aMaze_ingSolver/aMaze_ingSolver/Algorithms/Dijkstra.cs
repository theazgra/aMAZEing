using System;
using System.Collections.Generic;
using System.Linq;
using aMaze_ingSolver.GraphUtils;

namespace aMaze_ingSolver.Algorithms
{
    class Dijkstra : MazeSolver
    {
        public override bool SupportParallel => false;
        public override string Name => "Dijkstra";
        public override event Solved OnSolved;
        public override event SolveProgress OnSolveProgress;

        private List<Vertex> _unvisited;
        private bool _invokeEvents = false;
        
        private object _unvisitedLock = new object();


        private void AddNewBestDistance(Vertex previous, Vertex current, float distance)
        {
            current.Previous = previous;
            current.BestPathDistance = distance;

            lock (_unvisitedLock)
            {
                if (!_unvisited.Contains(current))
                {
                    _unvisited.Add(current);
                }
                _unvisited = _unvisited.OrderBy(v => v.BestPathDistance).ToList();
            }
        }

        private Vertex GetTopUnvisitedVertex()
        {
            lock (_unvisitedLock)
            {
                if (_unvisited.Count > 0)
                    return _unvisited.First();
            }
            return null;
        }


        public override void SolveMaze(Graph graph)
        {
            if (Parallel)
            {
                ParallelSolution(graph);
            }
            else
            {
                _invokeEvents = MazeForm.InvokeDelegates();
                long loopIteration = 0;
                int vertCount = graph.Vertices.Count;
                int invokeStep = (int)Math.Ceiling((float)(vertCount / 2000));
                graph.Reset();
                _resultPath.Clear();

                _timer.Start();

                _unvisited = new List<Vertex>();
                AddNewBestDistance(null, graph.Start, 0);
                Vertex current = null;

                //Start by adding start' neighbours
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

#if false
                //Slower.
                System.Threading.Tasks.Parallel.ForEach(current.Neighbours, (neighbour) =>
                {
                    if (!neighbour.Visited)
                    {

                        //distance to connected vertex using best distance to current vertex.
                        float distanceToVertex = current.BestPathDistance + current.PathDistanceTo(neighbour);
                        if (distanceToVertex < neighbour.BestPathDistance)
                        {
                            AddNewBestDistance(current, neighbour, distanceToVertex);
                        }
                    }
                });
#endif
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
                    ///
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
            }
        }

        private void ParallelSolution(Graph graph)
        {
        }

        private void TaskJob(object obj)
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
