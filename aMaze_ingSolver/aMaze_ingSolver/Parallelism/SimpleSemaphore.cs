using System;
using System.Collections.Generic;

namespace aMaze_ingSolver.Parallelism
{
    class SimpleSemaphore
    {
        private object _lock = new object();

        private int _maxThreadCount;
        private Queue<int> _tokens;
        private List<int> _leasedTokens;

        public SimpleSemaphore(int threadCount)
        {
            _maxThreadCount = threadCount;
            _tokens = new Queue<int>();
            _leasedTokens = new List<int>();

            for (int i = 0; i < _maxThreadCount; i++)
            {
                _tokens.Enqueue(i);
            }

        }

        public bool HasFreeToken()
        {
            lock (_lock)
            {
                return _tokens.Count > 0;
            }
        }

        public int GetToken()
        {
            lock (_lock)
            {
                if (_tokens.Count <= 0)
                    throw new Exception("No thread is avaible!");

                int token = _tokens.Dequeue();
                _leasedTokens.Add(token);
                return token;
            }
        }

        public void ReturnToken(int token)
        {
            lock (_lock)
            {
                if (_tokens.Count < _maxThreadCount)
                {
                    if (!_leasedTokens.Contains(token))
                    {
                        throw new Exception("This token was not leased.");
                    }
                    _leasedTokens.Remove(token);
                    _tokens.Enqueue(token);
                }
                else
                {
                    throw new Exception("There are no leased tokens.");
                } 
            }
        }
    }
}
