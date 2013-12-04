using System;
using System.Collections.Generic;
using System.Threading;

namespace Sparrow.Domain.Events
{
    public static class DomainEvents
    {
        private static readonly List<Delegate> _eventActions = new List<Delegate>();
        private static readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();

        #region ExecuteRead/Write

        private static void ExecuteRead(Action callback)
        {
            _locker.EnterReadLock();
            try
            {
                callback();
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        private static void ExecuteWrite(Action callback)
        {
            _locker.EnterWriteLock();
            try
            {
                callback();
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        #endregion

        /// <summary>
        /// Registers to an event with specified arguments.
        /// </summary>
        /// <returns>
        /// An <see cref="IDisposable"/> which can be used to unregister from the event.
        /// </returns>
        public static IDisposable Register<T>(Action<T> callback)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            ExecuteWrite(() => _eventActions.Add(callback));

            return new DisposableInvoker(() => 
                ExecuteWrite(() => _eventActions.Remove(callback)));
        }

        /// <summary>
        /// Raises the event with specified arguments.
        /// </summary>
        public static void Raise<T>(T eventArgs)
        {
            Delegate[] actions = null;
            ExecuteRead(() =>
            {
                actions = _eventActions.ToArray();
            });

            foreach (var action in actions)
            {
                var typedAction = action as Action<T>;
                if (typedAction != null)
                    typedAction(eventArgs);
            }
        }

        private sealed class DisposableInvoker : IDisposable
        {
            private readonly Action _disposeAction;

            public DisposableInvoker(Action disposeAction)
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