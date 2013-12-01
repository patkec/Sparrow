using System.Runtime.InteropServices;
using System.Web.Http;
using NHibernate;
using Sparrow.Infrastructure.Commands;

namespace Sparrow.Web.Infrastructure
{
    public class SessionApiController: ApiController
    {
        private CommandExecutor _commandExecutor;

        /// <summary>
        /// Gets the NHibernate session for current request.
        /// </summary>
        protected internal ISession Session { get; internal set; }

        protected ICommandExecutor CommandExecutor
        {
            get
            {
                if (_commandExecutor == null)
                    _commandExecutor = new CommandExecutor(Session);
                return _commandExecutor;
            }
        }
    }
}