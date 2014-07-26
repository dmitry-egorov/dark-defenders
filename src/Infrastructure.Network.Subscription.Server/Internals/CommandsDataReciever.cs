using Infrastructure.Network.Interfaces;
using Infrastructure.Network.Subscription.Server.Interfaces;
using Infrastructure.Runtime;

namespace Infrastructure.Network.Subscription.Server.Internals
{
    internal class CommandsDataReciever : IDataReciever
    {
        private readonly ICommandsDataInterpreter _interpreter;
        private readonly ActionProcessor _processor;

        public CommandsDataReciever(ICommandsDataInterpreter interpreter, ActionProcessor processor)
        {
            _interpreter = interpreter;
            _processor = processor;
        }

        public void Recieve(byte[] data)
        {
            var command = _interpreter.Interpret(data);

            _processor.Publish(command);
        }
    }
}