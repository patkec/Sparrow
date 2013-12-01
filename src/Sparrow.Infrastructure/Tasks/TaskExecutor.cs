using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;

namespace Sparrow.Infrastructure.Tasks
{
    public static class TaskExecutor
    {
        private static readonly ThreadLocal<List<BackgroundTask>> _tasksToExecute =
            new ThreadLocal<List<BackgroundTask>>(() => new List<BackgroundTask>());

        /// <summary>
        /// Gets or sets a <see cref="ISessionFactory"/> used when executing tasks.
        /// </summary>
        public static ISessionFactory SessionFactory { get; set; }

        /// <summary>
        /// Gets or sets a general exception handler.
        /// </summary>
        public static Action<Exception> OnException { get; set; }

        /// <summary>
        /// Queues the specified task for execution.
        /// </summary>
        /// <param name="task"><see cref="BackgroundTask"/> to be executed.</param>
        public static void ExecuteLater(BackgroundTask task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            _tasksToExecute.Value.Add(task);
        }

        /// <summary>
        /// Discards any remaining background tasks in current thread.
        /// </summary>
        public static void Discard()
        {
            _tasksToExecute.Value.Clear();
        }

        /// <summary>
        /// Start executing background tasks queued with <see cref="ExecuteLater"/>.
        /// </summary>
        public static void StartExecuting()
        {
            var tasksToExecute = _tasksToExecute.Value;
            var tasksToExecuteCopy = tasksToExecute.ToArray();
            tasksToExecute.Clear();

            if (tasksToExecuteCopy.Length > 0)
            {
                Task.Factory.StartNew(() =>
                {
                    foreach (var backgroundTask in tasksToExecuteCopy)
                    {
                        ExecuteTask(backgroundTask);
                    }
                }, TaskCreationOptions.LongRunning)
                    .ContinueWith(task =>
                    {
                        if (OnException != null)
                            OnException(task.Exception);
                    }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        /// <summary>
        /// Immediately executes given background task.
        /// </summary>
        /// <param name="task"><see cref="BackgroundTask"/> to execute.</param>
        public static void ExecuteTask(BackgroundTask task)
        {
            if (task == null)
                throw new ArgumentNullException("task");
            if (SessionFactory == null)
                throw new InvalidOperationException("SessionFactory is not initialized.");

            // If task execution fails then retry a number of times.
            for (int i = 0; i < 10; i++)
            {
                using (var session = SessionFactory.OpenSession())
                {
                    switch (task.Run(session))
                    {
                        case true:
                        case false:
                            return;
                        case null:
                            break;
                    }
                }
            }
        }

    }
}