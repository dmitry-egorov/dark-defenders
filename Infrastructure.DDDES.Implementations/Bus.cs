using System;
using System.Collections.Generic;
using Infrastructure.Concurrent;
using Infrastructure.Util;
using MoreLinq;

namespace Infrastructure.DDDES.Implementations
{
    public class Bus: IBus
    {
        private readonly ConcurrentHashSet<IObserver<IEnumerable<IEvent>>> _observers = new ConcurrentHashSet<IObserver<IEnumerable<IEvent>>>();

        private readonly ICommandProcessor _processor;

        public Bus(ICommandProcessor processor)
        {
            _processor = processor;
        }

        public void PublishTo<TRoot>(Identity id, Func<TRoot, IEnumerable<IEvent>> command)
        {
            var events = _processor.Process(id, command);

            PublishEvents(events);
        }

        public void PublishToAllOfType<T>(Func<T, IEnumerable<IEvent>> command)
        {
            var events = _processor.ProcessAllImplementing(command);

            PublishEvents(events);
        }

        public IDisposable Subscribe(IObserver<IEnumerable<IEvent>> observer)
        {
            if (!_observers.TryAdd(observer))
            {
                throw new InvalidOperationException("This instance of observer is already added");
            }

            return new ObserverUnsubscriper(_observers, observer);
        }

        private void PublishEvents(IEnumerable<IEvent> events)
        {
            var observers = _observers.GetAll();

            var eventsReadOnly = events.AsReadOnly();

            observers.ForEach(obs => obs.OnNext(eventsReadOnly));
        }

        private class ObserverUnsubscriper : IDisposable
        {
            private readonly ConcurrentHashSet<IObserver<IEnumerable<IEvent>>> _observers;
            private readonly IObserver<IEnumerable<IEvent>> _observer;

            public ObserverUnsubscriper(ConcurrentHashSet<IObserver<IEnumerable<IEvent>>> observers, IObserver<IEnumerable<IEvent>> observer)
            {
                _observers = observers;
                _observer = observer;
            }
            
            public void Dispose()
            {
                _observers.TryRemove(_observer);
            }
        }
    }
}