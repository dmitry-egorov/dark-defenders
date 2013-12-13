using System;
using System.Collections.ObjectModel;

namespace DarkDefenders.Infrastructure.CommandHandling.Direct
{
    public class DirectBus: IBus
    {
        private readonly ReadOnlyDictionary<Type, Action<ICommand>> _handlers;

        public DirectBus(ReadOnlyDictionary<Type, Action<ICommand>> handlers)
        {
            _handlers = handlers;
        }

        public void Publish(ICommand command)
        {
            Action<ICommand> action;
            if (!_handlers.TryGetValue(command.GetType(), out action))
            {
                throw new ApplicationException("Unknown command");
            }

            action(command);
        }
    }
}