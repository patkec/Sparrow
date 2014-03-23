using System.Web.Http;
using NHibernate;
using Sparrow.Infrastructure.Commands;

namespace Sparrow.Api.Infrastructure
{
    public class SessionApiController: ApiController, ISessionController
    {
        private CommandExecutor _commandExecutor;

        /// <summary>
        /// Gets the NHibernate session for current request.
        /// </summary>
        protected ISession Session {
            get { return ((ISessionController) this).Session; }
        }

        ISession ISessionController.Session { get; set; }

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