using DarkDefenders.Mono.Client.ScreenManagers;
using DarkDefenders.Mono.Client.Screens.Gameplay;

namespace DarkDefenders.Mono.Client.Screens
{
    public class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
        {
            // Add entries to the menu.
            MenuEntries.Add("START GAME");
            MenuEntries.Add("QUIT");
        }

        protected override void OnSelectEntry(int entryIndex)
        {
            if (entryIndex == 0)
            {
                ScreenManager.AddScreen(new GameplayScreen());
            }
            else if(entryIndex == 1)
            {
                ScreenManager.Game.Exit();
            }
        }

        protected override void OnCancel()
        {
            ScreenManager.Game.Exit();
        }
    }
}