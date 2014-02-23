using System;

namespace Infrastructure.Network.Subscription.Server.Interfaces
{
    public interface ICommandsDataInterpreter
    {
        Action Interpret(byte[] data);
    }
}