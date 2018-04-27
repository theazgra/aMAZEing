using aMaze_ingSolver.GraphUtils;
using aMaze_ingSolver.Parallelism;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace aMaze_ingSolver.Algorithms
{
    class DepthFirst : MazeSolver
    {
        public override bool SupportParallel => true;
        private Graph _graph;
        private List<Task> _tasks;
        private BlockingStackJobInfo[] _blockingStacks;
        CancellationToken _cancellationToken;
        CancellationTokenSource _cancellationTokenSource;
        private int _nextQId = 0;


        public override string Name => "Depth first";
        public override event Solved OnSolved;

        public DepthFirst()
        {
            _tasks = new List<Task>();
        }


        public override void SolveMaze(Graph graph)
        {
            if (!Parallel)
            {
                NonParallelSolution(graph);
            }
            else
            {
                ParallelSolution(graph);
            }
        }

        private async void ParallelSolution(Graph graph)
        {
            _graph = graph;
            graph.Reset();
            _timer.Start();

            _blockingStacks = new BlockingStackJobInfo[ThreadCount];
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;


            for (int i = 0; i < ThreadCount; i++)
            {
                int index = i;
                _blockingStacks[i] = new BlockingStackJobInfo(new ConcurrentStack<VertexPair>(), _cancellationToken);
                _tasks.Add(Task.Factory.StartNew(() => ParallelJob(_blockingStacks[index])));
            }

            _blockingStacks[NextQueueId()].blockingCollection.Push(new VertexPair(null, graph.Start));
            await Task.WhenAll(_tasks);

            _resultPath.Clear();
            Vertex current = _graph.End;
            while (current != null)
            {
                _resultPath.Enqueue(current);
                current = current.Previous;
            }

            _timer.Stop();
            OnSolved?.Invoke();
        }

        private void ParallelJob(BlockingStackJobInfo jobInfo)
        {
            VertexPair pair = null;
            while (!jobInfo.cancelToken.IsCancellationRequested)
            {
                try
                {
                    if (!jobInfo.blockingCollection.TryPop(out pair))
                        continue;

                }
                catch (OperationCanceledException)
                {
                    continue;
                }
                pair.Next.Previous = pair.Current;
                pair.Next.Visited = true;

                if (pair.Next.Equals(_graph.End))
                {
                    _cancellationTokenSource.Cancel();
                }
                else
                {
                    foreach (Vertex neighbour in pair.Next.GetOrderedNeighbours())
                    {
                        //if (!IsVisited(neighbour))
                        if (!neighbour.Visited)
                        {
                            neighbour.Visited = true;
                            neighbour.Previous = pair.Current;

                            _blockingStacks[NextQueueId()].blockingCollection.Push(new VertexPair(pair.Next, neighbour));

                        }
                    }
                }
            }

            while (jobInfo.blockingCollection.Count > 0)
            {
                if (!jobInfo.blockingCollection.TryPop(out pair))
                    continue;
                if (pair.Next.Equals(_graph.End))
                {
                    break;
                }

                foreach (Vertex neighbour in pair.Next.GetOrderedNeighbours())
                {
                    if (!neighbour.Visited)
                    {
                        neighbour.Visited = true;
                        neighbour.Previous = pair.Current;
                        jobInfo.blockingCollection.Push(new VertexPair(pair.Next, neighbour));
                    }
                }
            }
        }

        private int NextQueueId()
        {
            int val = _nextQId;
            _nextQId = (_nextQId + 1) % ThreadCount;
            return val;
        }

        private void NonParallelSolution(Graph graph)
        {
            Stack<Vertex> queue = new Stack<Vertex>();
            _timer.Start();

            queue.Push(graph.Start);

            graph.Start.Visited = true;
            graph.Start.Previous = null;

            int loopIteration = 0;

            Vertex current = null;
            while (true)
            {
                ++loopIteration;
                current = queue.Pop();

                if (current.Equals(graph.End))
                {
                    break;
                }

                foreach (Vertex neighbour in current.GetOrderedNeighbours())
                {
                    if (!neighbour.Visited)
                    {
                        queue.Push(neighbour);
                        neighbour.Visited = true;
                        neighbour.Previous = current;
                    }
                }
            }

            _resultPath.Clear();
            current = graph.End;

            while (current != null)
            {
                _resultPath.Enqueue(current);
                current = current.Previous;
            }

            _timer.Stop();
            OnSolved?.Invoke();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

