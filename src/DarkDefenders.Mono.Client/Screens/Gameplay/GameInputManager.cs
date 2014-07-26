using DarkDefenders.Game.App.Interfaces;
using DarkDefenders.Game.Model.Other;
using DarkDefenders.Mono.Client.ScreenManagers;
using Microsoft.Xna.Framework.Input;

namespace DarkDefenders.Mono.Client.Screens.Gameplay
{
    public class GameInputManager
    {
        private readonly IPlayerService _player;

        public GameInputManager(IPlayerService player)
        {
            _player = player;
        }

        public void HandleInput(InputState input)
        {
            var keyboardState = input.CurrentKeyboardState;

            var leftIsPressed = keyboardState.IsKeyDown(Keys.Left);
            var rightIsPressed = keyboardState.IsKeyDown(Keys.Right);
            if (leftIsPressed && !rightIsPressed)
            {
                _player.ChangeMovement(Movement.Left);
            }
            else if (rightIsPressed && !leftIsPressed)
            {
                _player.ChangeMovement(Movement.Right);
            }
            else
            {
                _player.ChangeMovement(Movement.Stop);
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                _player.Jump();
            }

            if (keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl))
            {
                _player.Fire();
            }

//            if (input.CurrentKeyboardState.IsKeyDown(Keys.Escape))
//            {
//                source.Cancel();
//            }
        }
    }
}