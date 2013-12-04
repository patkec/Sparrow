using System;
using NHibernate;

namespace Sparrow.Infrastructure.Tasks
{
    public abstract class BackgroundTask
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets the <see cref="ISession"/> for the task.
        /// </summary>
        protected ISession Session { get; private set; }

        /// <summary>
        /// Gets the logger for the current task.
        /// </summary>
        protected NLog.Logger Logger
        {
            get { return _logger; }
        }

        /// <summary>
        /// Initializes the task.
        /// </summary>
        protected virtual void Initialize(ISession session)
        {
            Session = session;
        }

        /// <summary>
        /// Called when an error occurs within the task.
        /// </summary>
        /// <param name="exception"><see cref="Exception"/> that represents the error.</param>
        protected virtual void OnError(Exception exception)
        {
        }

        /// <summary>
        /// Runs current task within the specified session.
        /// </summary>
        /// <param name="session"><see cref="ISession"/> in which the task is running.</param>
        /// <returns><see langword="true"/> if task completed successfuly, <see langword="false"/> if it completed unsuccessfuly; otherwise, <see langword="null"/>.</returns>
        public bool? Run(ISession session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            Initialize(session);
            try
            {
                using (var tx = session.BeginTransaction())
                {
                    Execute();
                    tx.Commit();
                }
                // Execute any tasks that were added to executor while executing current task.
                TaskExecutor.StartExecuting();
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error processing task " + GetType().Name, ex);
                OnError(ex);
                return false;
            }
            finally
            {
                TaskExecutor.Discard();
            }
        }

        /// <summary>
        /// Actually executes the task.
        /// </summary>
        protected abstract void Execute();
    }
}