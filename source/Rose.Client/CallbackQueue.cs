using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rose.Client
{
    internal class CallbackQueue
    {
        private Queue<Action> _queue = new Queue<Action>();





        internal void Enqueue(Action action)
        {
            lock (_queue)
                _queue.Enqueue(action);
        }


        public void Update()
        {
            lock (_queue)
            {
                while (_queue.Count > 0)
                {
                    var action = _queue.Dequeue();
                    action();
                }
            }
        }
    }
}
