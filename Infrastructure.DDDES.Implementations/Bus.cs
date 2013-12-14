using System;
using System.Collections.Generic;
using Infrastructure.Concurrent;
using MoreLinq;

namespace Infrastructure.DDDES.Implementations
{
    public class Bus: IBus
    {
        private readonly ConcurrentHashSet<IObserver<IEvent>> _observers = new ConcurrentHashSet<IObserver<IEvent>>();

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

        private void PublishEvents(IEnumerable<IEvent> events)
        {
            var observers = _observers.GetAll();

            events.ForEach(e => observers.ForEach(obs => obs.OnNext(e)));
        }

        public void PublishToAllOfType<T>(Func<T, IEnumerable<IEvent>> command)
        {
            var events = _processor.ProcessAllImplementing(command);

            PublishEvents(events);
        }

        public IDisposable Subscribe(IObserver<IEvent> observer)
        {
            if (!_observers.TryAdd(observer))
            {
                throw new InvalidOperationException("This instance of observer is already added");
            }

            return new ObserverUnsubscriper(_observers, observer);
        }

        private class ObserverUnsubscriper : IDisposable
        {
            private readonly ConcurrentHashSet<IObserver<IEvent>> _observers;
            private readonly IObserver<IEvent> _observer;

            public ObserverUnsubscriper(ConcurrentHashSet<IObserver<IEvent>> observers, IObserver<IEvent> observer)
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