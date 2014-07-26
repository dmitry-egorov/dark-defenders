using DarkDefenders.Game.Resources.Internals;
using DarkDefenders.Remote.Model;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkDefenders.Mono.Client.Presenters
{
    public class GamePresenter : IRemoteEvents
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Texture2D _whiteTexture;

        private readonly SpriteBatch _spriteBatch;
        private readonly AllEntitiesPresenter _entitiesPresenter;

        private volatile bool _loaded;
        private volatile TerrainPresenter _terrainPresenter;

        public GamePresenter(GraphicsDevice graphicsDevice, Texture2D whiteTexture)
        {
            _graphicsDevice = graphicsDevice;
            _whiteTexture = whiteTexture;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _entitiesPresenter = new AllEntitiesPresenter(_spriteBatch, whiteTexture);
        }

        public void MapLoaded(string mapId)
        {
            var data = WorldLoader.LoadFromFile(mapId, "Content");

            _terrainPresenter = new TerrainPresenter(data.Map, _whiteTexture, _spriteBatch);

            _loaded = true;
        }

        public void Destroyed(IdentityOf<RemoteEntity> id)
        {
            _entitiesPresenter.Remove(id);
        }

        public void Moved(IdentityOf<RemoteEntity> id, Vector newPosition)
        {
            _entitiesPresenter.ChangePosition(id, newPosition);
        }

        public void Created(IdentityOf<RemoteEntity> id, Vector initialPosition, RemoteEntityType type)
        {
            _entitiesPresenter.CreateNewEntity(id, initialPosition, type);
        }

        public void Present()
        {
            if (!_loaded)
            {
                return;
            }
            
            var projection = CreateProjectionMatrix();

            _spriteBatch.Begin(0, null, null, null, RasterizerState.CullClockwise, null, projection);

            _terrainPresenter.DrawTerrain();
            _entitiesPresenter.DrawEntities();

            _spriteBatch.End();
        }

        private Matrix CreateProjectionMatrix()
        {
            var viewport = _graphicsDevice.Viewport;

            return Matrix.CreateTranslation(-50f, -40f, 0)
                 * Matrix.CreateScale(8000.0f / viewport.Height)
                 * Matrix.CreateScale(1, -1, 1)
                 * Matrix.CreateTranslation(viewport.Width / 2.0f, viewport.Height / 2.0f, 0.0f);
        }
    }
}