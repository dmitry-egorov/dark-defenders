using System;
using System.Collections.Concurrent;
using System.Linq;
using Infrastructure.Util;

namespace Infrastructure.Runtime
{
    public class ActionProcessor<TExecuter>
    {
        private readonly ConcurrentQueue<Action<TExecuter>> _eventsQueue = new ConcurrentQueue<Action<TExecuter>>();
        private readonly int _singleProcessingActionsLimit;

        public ActionProcessor(int singleProcessingActionsLimit = int.MaxValue)
        {
            _singleProcessingActionsLimit = singleProcessingActionsLimit;
        }

        public void Publish(Action<TExecuter> action)
        {
            _eventsQueue.Enqueue(action);
        }

        public void Process(TExecuter executer)
        {
            var actions = _eventsQueue.DequeueAllCurrent().Take(_singleProcessingActionsLimit).AsReadOnly();

            foreach (var action in actions)
            {
                action(executer);
            }
        }
    }

    public class ActionProcessor
    {
        private readonly ConcurrentQueue<Action> _eventsQueue = new ConcurrentQueue<Action>();
        private readonly int _singleProcessingActionsLimit;

        public ActionProcessor(int singleProcessingActionsLimit = int.MaxValue)
        {
            _singleProcessingActionsLimit = singleProcessingActionsLimit;
        }

        public void Publish(Action action)
        {
            _eventsQueue.Enqueue(action);
        }

        public void Process()
        {
            var actions = _eventsQueue.DequeueAllCurrent().Take(_singleProcessingActionsLimit).AsReadOnly();

            foreach (var action in actions)
            {
                action();
            }
        }
    }
}