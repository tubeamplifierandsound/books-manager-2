using BooksManager.Lib.Concurrent;

namespace BooksManager.Tests
{
    [TestClass]
    public sealed class TaskQueueTests
    {
        [TestMethod]
        public void EnqueueTask_WhenTaskEnqueue_ShouldExecuteTask() {
            // Average
            int counter = 0;
            TaskQueue.TaskDelegate task = () =>
            {
                Interlocked.Increment(ref counter);
            };
            var taskQueue = new TaskQueue(2);
            // Act
            taskQueue.EnqueueTask(task);
            
            Thread.Sleep(1000);
            taskQueue.StopPool();
            // Assert
            Assert.AreEqual(1, counter);
        }

        [TestMethod]
        public void StopPool_WhenStopBeforeTaskEnqueue_ShouldNotExecuteTaskAfterStop()
        {
            // Average
            int counter = 0;
            TaskQueue.TaskDelegate task = () =>
            {
                Interlocked.Increment(ref counter);

            };
            var taskQueue = new TaskQueue(2);
            taskQueue.EnqueueTask(task);
            // Act
            taskQueue.StopPool();
            taskQueue.EnqueueTask(task);
            Thread.Sleep(1000);
            // Assert
            Assert.AreEqual(1, counter);
        }
    }
}
