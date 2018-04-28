using aMaze_ingSolver.GraphUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace aMaze_ingSolver.Algorithms
{
    class LeftTurn : MazeSolver
    {
        public override bool SupportParallel => true;
        public override bool SupportThreadCount => false;

        public override string Name => "Left turn";

        public override event Solved OnSolved;
        private CancellationTokenSource _cancellationTokenSource;

        private List<PossibleWayInfo> _possibleWays;
        private List<Task> _tasks;
        private Graph _graph;

        public override void SolveMaze(Graph graph)
        {
            graph.Reset();

            if (!Parallel)
                NonParallelSolution(graph);
            else
                ParallelSolution(graph);
        }

        private async void ParallelSolution(Graph graph)
        {
            ThreadCount = 3;
            _graph = graph;

            _possibleWays = new List<PossibleWayInfo>();
            _tasks = new List<Task>();
            _timer.Start();
            _resultPath = new Queue<Vertex>();
            _resultPath.Enqueue(graph.Start);
            graph.Start.Visited = true;

            //current after start.
            Vertex current = graph.Start.GetNeighbour(Direction.Down);
            Direction currentDirection = Direction.Down;

            while (true)
            {
                _resultPath.Enqueue(graph.Start);

                //End condition.
                if (current.Equals(graph.End))
                {
                    _timer.Stop();
                    _resultPath = new Queue<Vertex>(_resultPath.Where(v => v != null));
                    OnSolved?.Invoke();
                    break;
                }

                if (current.NeighbourCount <= 1)
                {
                    GetNextVertex(ref current, ref currentDirection);
                }
                else
                {
                    _possibleWays.Clear();
                    _tasks.Clear();

                    _cancellationTokenSource = new CancellationTokenSource();
                    int i = -1;
                    foreach (OrientedEdge edge in current.Edges)
                    {
                        if (edge.Destination.Visited)
                            continue;

                        PossibleWayInfo pwi = new PossibleWayInfo(current, edge.Direction, _cancellationTokenSource.Token);
                        lock (_possibleWays)
                        {
                            _possibleWays.Add(pwi); 
                        }
                        _tasks.Add(Task.Factory.StartNew(() => FindPossibleWay(pwi)));
                    }

                    await Task.WhenAll(_tasks);
                    PossibleWayInfo goodWay;
                    if (_cancellationTokenSource.IsCancellationRequested)
                    {
                        lock (_possibleWays)
                            goodWay = _possibleWays.Single(w => w != null && w.cancelSource);
                    }
                    else
                    {
                        lock (_possibleWays)
                            goodWay = _possibleWays.Single(w => w != null && !w.deadEnd);
                    }

                    
                    while (goodWay.pathFromOrigin.Count != 0)
                    {
                        _resultPath.Enqueue(goodWay.pathFromOrigin.Dequeue());
                    }
                    current = goodWay.lastVertex;
                }
            }
        }

        private bool ReducePossibleWays()
        {
            int cancelled = -1;
            lock (_possibleWays)
            {
                cancelled = 0;
                lock (_possibleWays)
                {
                    foreach (PossibleWayInfo pwi in _possibleWays)
                    {
                        if (pwi.deadEnd)
                            ++cancelled;
                    } 
                }
            }

            //All except one are cancelled.
            return (_tasks.Count - 1 == cancelled);
        }

        private void FindPossibleWay(PossibleWayInfo possibleWayInfo)
        {
            Vertex current = possibleWayInfo.origin;
            Direction currentDirection = possibleWayInfo.direction;
            current = current.GetNeighbour(currentDirection);

            while (!possibleWayInfo.cancellationToken.IsCancellationRequested)
            {
                possibleWayInfo.pathFromOrigin.Enqueue(current);

                if (current.Equals(_graph.End))
                {
                    possibleWayInfo.cancelSource = true;
                    _cancellationTokenSource.Cancel();
                    break;
                }

                GetNextVertex(ref current, ref currentDirection);

                possibleWayInfo.lastVertex = current;

                if (current.Equals(possibleWayInfo.origin))
                {
                    possibleWayInfo.deadEnd = true;
                    break;
                }

                if (ReducePossibleWays())
                    break;
            }
        }

        private void NonParallelSolution(Graph graph)
        {
            _timer.Start();
            _resultPath = new Queue<Vertex>();

            _resultPath.Enqueue(graph.Start);

            Vertex current = graph.Start.GetNeighbour(Direction.Down);

            Direction currentDirection = Direction.Down; //2

            while (true)
            {
                _resultPath.Enqueue(current);

                if (current.Equals(graph.End))
                {
                    _timer.Stop();
                    OnSolved?.Invoke();
                    break;
                }

               bool found =  GetNextVertex(ref current, ref currentDirection);
            }
        }

        /// <summary>
        /// Get next vertex, or dead end.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="currentDirection"></param>
        /// <returns>False if dead end.</returns>
        private bool GetNextVertex(ref Vertex current, ref Direction currentDirection, bool markVisited = false)
        {
            if (markVisited)
            {
                current.Visited = true; 
            }

            Vertex neighbour;

            Direction direction = (Direction)Utils.Modulo((int)currentDirection - 1, 4);
            if (current.HasNeighbour(direction))
            {
                neighbour = current.GetNeighbour(direction);
                if (neighbour.Visited)
                    return false;

                currentDirection = direction;
                current = neighbour;
                return true;
            }

            if (current.HasNeighbour(currentDirection))
            {
                neighbour = current.GetNeighbour(currentDirection);
                if (neighbour.Visited)
                    return false;
                current = neighbour;
                return true;
            }

            direction = (Direction)(((int)currentDirection + 1) % 4);
            if (current.HasNeighbour(direction))
            {
                neighbour = current.GetNeighbour(direction); 
                if (neighbour.Visited)
                    return false;
                currentDirection = direction;
                current = neighbour;
                return true; 
            }

            direction = (Direction)(((int)currentDirection + 2) % 4);
            if (current.HasNeighbour(direction))
            {
                neighbour = current.GetNeighbour(direction);
                if (neighbour.Visited)
                    return false;

                currentDirection = direction;
                current = neighbour;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
