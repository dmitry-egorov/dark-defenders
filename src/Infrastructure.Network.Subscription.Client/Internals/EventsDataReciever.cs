using System;
using System.IO;
using System.Runtime.Serialization;
using Infrastructure.Network.Interfaces;
using Infrastructure.Network.Subscription.Client.Interfaces;
using Infrastructure.Runtime;
using Infrastructure.Serialization;

namespace Infrastructure.Network.Subscription.Client.Internals
{
    internal class EventsDataReciever : IDataReciever
    {
        private readonly IEventsDataInterpreter _interpreter;
        private readonly ActionProcessor _processor;

        public EventsDataReciever(IEventsDataInterpreter interpreter, ActionProcessor processor)
        {
            _interpreter = interpreter;
            _processor = processor;
        }

        public void Recieve(byte[] data)
        {
            var action = data.UsingGZipBinaryReader(reader => Read(reader));

            _processor.Publish(action);
        }

        private Action Read(BinaryReader reader)
        {
            var type = (SubscriptionDataType) reader.ReadByte();

            if (type == SubscriptionDataType.InitialState)
            {
                return _interpreter.Interpret(reader);
            }
            
            if (type == SubscriptionDataType.Update)
            {
                return _interpreter.Interpret(reader);
            }
            
            throw new SerializationException("Unknown subscription data type");
        }
    }
}