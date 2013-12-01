namespace Sparrow.Infrastructure.Commands
{
    public interface ICommandExecutor
    {
        /// <summary>
        /// Immediately executes the specified command.
        /// </summary>
        /// <param name="command"><see cref="Command"/> to be executed.</param>
        void ExecuteCommand(Command command);
    }
}