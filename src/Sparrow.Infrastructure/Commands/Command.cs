using System;
using NHibernate;

namespace Sparrow.Infrastructure.Commands
{
    public abstract class Command
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets the <see cref="ISession"/> for the command.
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
        /// Initializes the command.
        /// </summary>
        protected virtual void Initialize(ISession session)
        {
            Session = session;
        }

        /// <summary>
        /// Called when an error occurs within the command.
        /// </summary>
        /// <param name="exception"><see cref="Exception"/> that represents the error.</param>
        protected virtual void OnError(Exception exception)
        {
        }

        /// <summary>
        /// Executes the command within the specified session.
        /// </summary>
        /// <param name="session"><see cref="ISession"/> in which the command is executing.</param>
        public bool Execute(ISession session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            Initialize(session);
            try
            {
                Execute();
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error executing command " + GetType().Name, ex);
                OnError(ex);
                return false;
            }
        }

        /// <summary>
        /// Actually executes the command.
        /// </summary>
        protected abstract void Execute();
    }
}