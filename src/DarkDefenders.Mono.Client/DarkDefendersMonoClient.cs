using DarkDefenders.Mono.Client.ScreenManagers;
using DarkDefenders.Mono.Client.Screens;
using Microsoft.Xna.Framework;

namespace DarkDefenders.Mono.Client
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DarkDefendersMonoClient : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _gdm;

        public DarkDefendersMonoClient()
        {
            _gdm = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1200, 
                PreferredBackBufferHeight = 900
            };
            Content.RootDirectory = "Content";

            var screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            screenManager.AddScreen(new BackgroundScreen());
            screenManager.AddScreen(new MainMenuScreen());
        }
    }
}
