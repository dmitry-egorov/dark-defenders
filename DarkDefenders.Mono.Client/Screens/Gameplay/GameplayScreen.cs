using System.Net;
using System.Threading;
using DarkDefenders.Game.Serialization;
using DarkDefenders.Mono.Client.Presenters;
using DarkDefenders.Mono.Client.ScreenManagers;
using DarkDefenders.Remote.Serialization;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Network.Subscription.Client;
using Infrastructure.Network.Subscription.Client.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkDefenders.Mono.Client.Screens.Gameplay
{
    public class GameplayScreen : GameScreen
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private ISubscriptionClient _client;
        private GamePresenter _presenter;
        private GameInputManager _inputManager;

        public override void LoadContent()
        {
            var whiteTexture = ScreenManager.Game.Content.Load<Texture2D>("White");

            _presenter = new GamePresenter(ScreenManager.Game.GraphicsDevice, whiteTexture);

            var listener = DelegatingEventsListener.Create(_presenter);

            var interpreter = new EventsDeserializer(listener);

            _client = SubscriptionClient.Create(interpreter, new IPAddress(new byte[]{192, 168, 1, 42}), 1337);

            var sender = _client.RunAsync(_cancellationTokenSource.Token);

            var playerService = new RemotePlayerService(sender);

            _inputManager = new GameInputManager(playerService);

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            _cancellationTokenSource.Cancel();

            base.UnloadContent();
        }

        public override void HandleInput(InputState input)
        {
            _inputManager.HandleInput(input);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _client.Pulse();

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            _presenter.Present();
        }
    }
}