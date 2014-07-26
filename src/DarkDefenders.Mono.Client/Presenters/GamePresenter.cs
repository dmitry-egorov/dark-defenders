using DarkDefenders.Game.Resources.Internals;
using DarkDefenders.Remote.Model;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Microsoft.Xna.Framework.Graphics;

namespace DarkDefenders.Mono.Client.Presenters
{
    public class GamePresenter : IRemoteEvents
    {
        private readonly Texture2D _whiteTexture;

        private readonly SpriteBatch _spriteBatch;
        private readonly AllEntitiesPresenter _entitiesPresenter;

        private volatile bool _loaded;
        private volatile TerrainPresenter _terrainPresenter;
        private readonly Camera _camera;
        private readonly PlayerFollowingOperator _operator;
        private readonly IResources<RemoteEntityType, EntityProperties> _resources;

        public GamePresenter(GraphicsDevice graphicsDevice, Texture2D whiteTexture, IResources<RemoteEntityType, EntityProperties> resources)
        {
            _whiteTexture = whiteTexture;
            _resources = resources;
            _spriteBatch = new SpriteBatch(graphicsDevice);

            _entitiesPresenter = CreateEntitiesPresenter();
            _camera = CreateCamera(graphicsDevice.Viewport);
            _operator = new PlayerFollowingOperator(_camera);
        }

        public void Update()
        {
            _operator.Update();
        }

        public void Present()
        {
            if (!_loaded)
            {
                return;
            }

            var projection = _camera.GetProjectionMatrix();

            _spriteBatch.Begin(0, null, SamplerState.PointClamp, null, RasterizerState.CullClockwise, null, projection);

            _terrainPresenter.Draw();
            _entitiesPresenter.Draw();

            _spriteBatch.End();
        }

        public void MapLoaded(string mapId)
        {
            var data = WorldLoader.LoadFromFile(mapId, "Content");

            _terrainPresenter = new TerrainPresenter(data.Map, _whiteTexture, _spriteBatch);

            _loaded = true;
        }

        public void Created(IdentityOf<RemoteEntity> id, Vector initialPosition, RemoteEntityType type)
        {
            _operator.NotifyCreated(id, initialPosition, type);
            _entitiesPresenter.CreateNewEntity(id, initialPosition, type);
        }

        public void Moved(IdentityOf<RemoteEntity> id, Vector newPosition)
        {
            _operator.NotifyMoved(id, newPosition);
            _entitiesPresenter.ChangePosition(id, newPosition);
        }

        public void ChangedDirection(IdentityOf<RemoteEntity> id, Direction newDirection)
        {
            _entitiesPresenter.ChangeDirection(id, newDirection);
        }

        public void Destroyed(IdentityOf<RemoteEntity> id)
        {
            _entitiesPresenter.Remove(id);
        }

        private AllEntitiesPresenter CreateEntitiesPresenter()
        {
            return new AllEntitiesPresenter(_spriteBatch, _resources);
        }

        private static Camera CreateCamera(Viewport viewport)
        {
            return new Camera(viewport.Width, viewport.Height, 20.0f, new Vector(50, 40));
        }
    }
}