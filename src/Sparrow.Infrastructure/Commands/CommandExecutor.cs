using System;
using NHibernate;

namespace Sparrow.Infrastructure.Commands
{
    public class CommandExecutor: ICommandExecutor
    {
        private readonly ISession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutor"/> class.
        /// </summary>
        /// <param name="session"><see cref="ISession"/> in which commands should be executed.</param>
        public CommandExecutor(ISession session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            _session = session;
        }

        /// <summary>
        /// Immediately executes the specified command.
        /// </summary>
        /// <param name="command"><see cref="Command"/> to be executed.</param>
        public void ExecuteCommand(Command command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            command.Execute(_session);
        }
    }
}