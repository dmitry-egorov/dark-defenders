using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DarkDefenders.Commands;
using DarkDefenders.Infrastructure.CommandHandling;
using DarkDefenders.Infrastructure.CommandHandling.Direct;
using NUnit.Framework;

namespace DarkDefenders.IntegrationTests
{
    [TestFixture]
    public class MainIntegrationTest
    {
        [Test]
        public void Should_work()
        {
            var actions = CreateActions();
            var bus = new DirectBus(actions);

            const int addPlayerRequestId = 1;
            bus.Publish(new AddPlayerCommand(addPlayerRequestId));

            bus.Publish(new SpawnAvatarCommand());
        }

        private static ReadOnlyDictionary<Type, Action<ICommand>> CreateActions()
        {
            var addPlayerCommandHandler = new AddPlayerCommand.Handler();
            var spawnAvatarCommandHandler = new SpawnAvatarCommand.Handler();

            var dictionary = new Dictionary<Type, Action<ICommand>>();

            RegisterHandler(dictionary, addPlayerCommandHandler);
            RegisterHandler(dictionary, spawnAvatarCommandHandler);

            return new ReadOnlyDictionary<Type, Action<ICommand>>(dictionary);
        }

        private static void RegisterHandler<T>(IDictionary<Type, Action<ICommand>> dictionary, ICommandHandler<T> handler) 
            where T : ICommand
        {
            dictionary.Add(typeof (T), CreateHandleAction(handler));
        }

        private static Action<ICommand> CreateHandleAction<T>(ICommandHandler<T> handler) where T : ICommand
        {
            return c => handler.Handle((T) c);
        }
    }
}
