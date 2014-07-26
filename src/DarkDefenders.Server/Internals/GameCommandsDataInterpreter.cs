using System;
using DarkDefenders.Game.App.Interfaces;
using DarkDefenders.Game.Model.Other;
using DarkDefenders.Game.Serialization;
using Infrastructure.Network.Subscription.Server.Interfaces;

namespace DarkDefenders.Server.Internals
{
    public class GameCommandsDataInterpreter : ICommandsDataInterpreter
    {
        private readonly IPlayerService _player;

        public GameCommandsDataInterpreter(IPlayerService player)
        {
            _player = player;
        }

        public Action Interpret(byte[] data)
        {
            var type = (PlayerCommandType)data[0];

            switch (type)
            {
                case PlayerCommandType.Stop:
                    return () => _player.ChangeMovement(Movement.Stop);
                case PlayerCommandType.Left:
                    return () => _player.ChangeMovement(Movement.Left);
                case PlayerCommandType.Right:
                    return () => _player.ChangeMovement(Movement.Right);
                case PlayerCommandType.Jump:
                    return () => _player.Jump();
                case PlayerCommandType.Fire:
                    return () => _player.Fire();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}