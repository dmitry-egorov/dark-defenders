using System;
using System.IO;

namespace Infrastructure.Network.Subscription.Client.Interfaces
{
    public interface IEventsDataInterpreter
    {
        Action InterpretInitial(BinaryReader reader);
        Action Interpret(BinaryReader reader);
    }
}