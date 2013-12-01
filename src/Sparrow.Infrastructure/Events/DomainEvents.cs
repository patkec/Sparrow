using System;
using System.Collections.Generic;
using System.Threading;

namespace Sparrow.Infrastructure.Events
{
    public static class DomainEvents
    {
        private static readonly ThreadLocal<List<Delegate>> _eventActions =
           new ThreadLocal<List<Delegate>>(() => new List<Delegate>());

        /// <summary>
        /// Registers to an event with specified arguments.
        /// </summary>
        /// <returns>
        /// An <see cref="IDisposable"/> which can be used to unregister from the event.
        /// </returns>
        public static IDisposable Register<T>(Action<T> callback)
        {
            _eventActions.Value.Add(callback);
            return new DomainEventRegistrationRemover(() => _eventActions.Value.Remove(callback));
        }

        /// <summary>
        /// Raises the event with specified arguments.
        /// </summary>
        public static void Raise<T>(T eventArgs)
        {
            var actions = _eventActions.Value.ToArray();

            foreach (var action in actions)
            {
                var typedAction = action as Action<T>;
                if (typedAction != null)
                    typedAction(eventArgs);
            }
        }

        private sealed class DomainEventRegistrationRemover : IDisposable
        {
            private readonly Action _disposeAction;

            public DomainEventRegistrationRemover(Action disposeAction)
            {
                _disposeAction = disposeAction;
            }

            public void Dispose()
            {
                _disposeAction();
            }
        }
    }
}