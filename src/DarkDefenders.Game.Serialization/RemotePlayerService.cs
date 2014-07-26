using System;
using DarkDefenders.Game.App.Interfaces;
using DarkDefenders.Game.Model.Other;
using Infrastructure.Network.Subscription.Client.Interfaces;

namespace DarkDefenders.Game.Serialization
{
    public class RemotePlayerService : IPlayerService
    {
        private readonly ICommandsDataSender _commandsDataSender;

        public RemotePlayerService(ICommandsDataSender commandsDataSender)
        {
            _commandsDataSender = commandsDataSender;
        }

        public void ChangeMovement(Movement movement)
        {
            switch (movement)
            {
                case Movement.Stop:
                    Send(PlayerCommandType.Stop);
                    break;
                case Movement.Left:
                    Send(PlayerCommandType.Left);
                    break;
                case Movement.Right:
                    Send(PlayerCommandType.Right);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("movement");
            }
        }

        public void Jump()
        {
            Send(PlayerCommandType.Jump);
        }

        public void Fire()
        {
            Send(PlayerCommandType.Fire);
        }

        private void Send(PlayerCommandType playerCommandType)
        {
            _commandsDataSender.TrySend(BitConverter.GetBytes((byte) playerCommandType));
        }
    }
}