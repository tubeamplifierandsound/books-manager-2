using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksManager.Lib.Concurrent
{
    public class TaskQueue
    {//mb add start method
        public delegate void TaskDelegate();

        public int ThreadNum { get;}

        private readonly Queue<TaskDelegate> _taskQueue = new Queue<TaskDelegate>();
        private readonly List<Thread> _threads = new List<Thread>();
        private readonly object _lockObj = new object();
        private bool _poolIsRunning = true;

        
        public TaskQueue(int threadNum)
        {
            ThreadNum = threadNum;
            for (int i = 0; i < threadNum; i++) {
                Thread newThread = new Thread(ThreadTask);
                // if true then thread can't block process termination
                newThread.IsBackground = true;
                _threads.Add(newThread);
                newThread.Start();
            }
        }

        public void EnqueueTask(TaskDelegate task) {
            if (task == null) {
                return;
            }
            lock (_lockObj) {
                _taskQueue.Enqueue(task);
                Monitor.Pulse(_lockObj);
            }
        }

        private void ThreadTask() {
            while (_poolIsRunning) {
                TaskDelegate task = null;
                lock (_lockObj) {
                    while (_poolIsRunning && _taskQueue.Count == 0) {
                        Monitor.Wait(_lockObj);
                    }
                    if (_poolIsRunning)
                    {
                        task = _taskQueue.Dequeue();
                    }
                    else {
                        return;
                    }
                }
                task?.Invoke();
            }
        }

        public void StopPool() {
            lock (_lockObj) { 
               _poolIsRunning = false;
                Monitor.PulseAll(_lockObj);
            }
        }

    }
}
