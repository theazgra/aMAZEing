using System;
using System.Collections.Generic;
using aMaze_ingSolver.GraphUtils;
using aMaze_ingSolver.Parallelism;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace aMaze_ingSolver.Algorithms
{
    class BreadthFirst : MazeSolver
    {
        public override bool SupportParallel => true;
        private SimpleSemaphore _semaphore;
        private object _taskLock = new object();

        private List<Task> _tasks;
        private BlockingQueueJobInfo[] _blockingQueus;
        CancellationToken _cancellationToken;
        CancellationTokenSource _cancellationTokenSource;
        private int _nextQId = 0;
        private Graph _graph;


        public override string Name => "Breadth first";
        public override event Solved OnSolved;

        public BreadthFirst()
        {
            _tasks = new List<Task>();
        }

        public override void SolveMaze(Graph graph)
        {
            _graph = graph;
            graph.Reset();
            if (!Parallel)
            {
                NonParallelSolution(graph);
            }
            else
            {
                ParallelSolution2(graph);
            }
        }

        private int NextQueueId()
        {
            int val = _nextQId;
            _nextQId = (_nextQId + 1) % ThreadCount;
            return val;
        }

        private async void ParallelSolution2(Graph graph)
        {
            graph.Reset();

            _timer.Start();

            _blockingQueus = new BlockingQueueJobInfo[ThreadCount];
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            
            
            for (int i = 0; i < ThreadCount; i++)
            {
                int index = i;
                _blockingQueus[i] = new BlockingQueueJobInfo(new ConcurrentQueue<VertexPair>(), _cancellationToken);
                _tasks.Add(Task.Factory.StartNew(() => ParallelJob(_blockingQueus[index])));
            }

            _blockingQueus[NextQueueId()].blockingCollection.Enqueue(new VertexPair(null, graph.Start));
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

       
        private void ParallelJob(BlockingQueueJobInfo jobInfo)
        {
            VertexPair pair = null;
            while (!jobInfo.cancelToken.IsCancellationRequested)
            {
                try
                {
                    if (!jobInfo.blockingCollection.TryDequeue(out pair))
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
                            
                            _blockingQueus[NextQueueId()].blockingCollection.Enqueue(new VertexPair(pair.Next, neighbour));
                            
                        }
                    }
                }
            }

            while (jobInfo.blockingCollection.Count > 0)
            {
                if (!jobInfo.blockingCollection.TryDequeue(out pair))
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
                        jobInfo.blockingCollection.Enqueue(new VertexPair(pair.Next, neighbour));
                    }
                }
            }
        }

        /// <summary>
        /// Old parallel solution.
        /// </summary>
        /// <param name="graph"></param>
        private void ParallelSolution(Graph graph)
        {
            _semaphore = new SimpleSemaphore(ThreadCount);
            int token = _semaphore.GetToken();
            VertexParam vp = new VertexParam(null, graph.Start, token);
            _timer.Reset();
            _timer.Start();
            StartThread(vp);


            while (true)
            {
                List<Task> toRemove = new List<Task>();
                IEnumerable<Task> check;
                lock (_taskLock)
                {
                    check = _tasks.ToArray();
                }
                foreach (Task task in check)
                {
                    if (task.IsCompleted)
                    {
                        toRemove.Add(task);
                    }
                }

                lock (_taskLock)
                {
                    _tasks.RemoveAll(t => toRemove.Contains(t));
                    if (_tasks.Count <= 0)
                        break;
                }
            }

            _resultPath.Clear();
            Vertex current = graph.End;

            while (current != null)
            {
                _resultPath.Enqueue(current);
                //current = _visited[current];
                current = current.Previous;
            }

            _timer.Stop();
            OnSolved?.Invoke();
            System.Console.WriteLine("Finished");
        }

        /// <summary>
        /// Used to start thread for old method.
        /// </summary>
        /// <param name="param"></param>
        private void StartThread(VertexParam param)
        {
            lock (_taskLock)
            {
                _tasks.Add(Task.Factory.StartNew(() => ThreadWork(param)));
            }
        }


        /// <summary>
        /// Work for old parallel solution.
        /// </summary>
        /// <param name="param"></param>
        private void ThreadWork(object param)
        {
            if (param is VertexParam vertexParam)
            {
                Queue<Vertex> queue = new Queue<Vertex>();
                queue.Enqueue(vertexParam.nextVertex);
                vertexParam.nextVertex.Visited = true;
                vertexParam.nextVertex.Previous = vertexParam.currentVertex;

                Vertex current = null;
                while (true)
                {
                    if (queue.Count <= 0)
                        break;

                    current = queue.Dequeue();

                    if (current.Equals(_graph.End))
                        break;

                    foreach (Vertex neighbour in current.GetOrderedNeighbours())
                    {
                        if (!neighbour.Visited)
                        {
                            neighbour.Visited = true;
                            neighbour.Previous = current;

                            if (_semaphore.HasFreeToken())
                            {
                                int token = _semaphore.GetToken();
                                VertexParam vp = new VertexParam(current, neighbour, token);
                                StartThread(vp);
                            }
                            else
                            {
                                queue.Enqueue(neighbour);
                            }
                        }
                    }
                }


                _semaphore.ReturnToken(vertexParam.threadToken);

            }
            else
            {
                throw new Exception("Thread is expectiong Vertex object as a parameter.");
            }
        }

        /// <summary>
        /// Standart solution.
        /// </summary>
        /// <param name="graph"></param>
        private void NonParallelSolution(Graph graph)
        {
            Queue<Vertex> queue = new Queue<Vertex>();
            _timer.Start();

            queue.Enqueue(graph.Start);
            graph.Start.Visited = true;
            graph.Start.Previous= null;

            Vertex current = null;
            while (true)
            {
                current = queue.Dequeue();

                if (current.Equals(graph.End))
                {
                    break;
                }

                foreach (Vertex neighbour in current.GetOrderedNeighbours())
                {
                    if (!neighbour.Visited)
                    {
                        queue.Enqueue(neighbour);
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
