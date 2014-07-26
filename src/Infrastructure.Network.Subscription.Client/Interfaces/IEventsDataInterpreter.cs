using System;
using System.IO;

namespace Infrastructure.Network.Subscription.Client.Interfaces
{
    public interface IEventsDataInterpreter
    {
        Action Interpret(BinaryReader reader);
    }
}