using System;
using System.Collections.Generic;

namespace Sparrow.Infrastructure.Events
{
    public static class DomainEvents
    {
        private static readonly List<Delegate> _eventActions = new List<Delegate>();

        /// <summary>
        /// Registers to an event with specified arguments.
        /// </summary>
        /// <returns>
        /// An <see cref="IDisposable"/> which can be used to unregister from the event.
        /// </returns>
        public static IDisposable Register<T>(Action<T> callback)
        {
            lock (_eventActions)
                _eventActions.Add(callback);
            
            return new DomainEventRegistrationRemover(() =>
            {
                lock (_eventActions)
                    _eventActions.Remove(callback);
            });
        }

        /// <summary>
        /// Raises the event with specified arguments.
        /// </summary>
        public static void Raise<T>(T eventArgs)
        {
            Delegate[] actions;
            lock (_eventActions)
                actions = _eventActions.ToArray();

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