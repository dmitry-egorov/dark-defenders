using System;
using System.Collections.Generic;
using System.Globalization;
using DarkDefenders.ConsoleClient.Renderer;
using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Other;
using DarkDefenders.Domain.Resources.Internals;
using DarkDefenders.Remote.Model.Interface;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.ConsoleClient.Presenters
{
    internal class GamePresenter : IRemoteEvents
    {
        private IConsoleRenderer _consoleRenderer;
        private readonly Dictionary<IdentityOf<RigidBody>, RigidBodyPresenter> _presentersMap = new Dictionary<IdentityOf<RigidBody>, RigidBodyPresenter>();

        private Vector _lastCreaturePosition = Vector.Zero;
        private IdentityOf<RigidBody> _playersRigidBodyId;
        private Map<Tile> _map;
        private int _totalHeroesSpawned;
        private bool _creaturesRenderingEnabled = true;

        public GamePresenter()
        {
            _consoleRenderer = new EmptyConsoleRenderer();
        }

        public void MapLoaded(string mapId)
        {
            var data = WorldLoader.LoadFromFile(mapId);

            _map = data.Map;

            SetViewPort();
            RenderWorld();
        }

        public void Created(IdentityOf<RigidBody> id, Vector position, RemoteEntityType type)
        {
            var presenter = new RigidBodyPresenter(_map, _consoleRenderer);

            _presentersMap.Add(id, presenter);

            presenter.RigidBodyCreated(position, type);

            if (type == RemoteEntityType.Hero)
            {
                _totalHeroesSpawned++;
                RenderHeroesCount();
            }
            else if(type == RemoteEntityType.Player)
            {
                _playersRigidBodyId = id;
            }
        }

        public void Destroyed(IdentityOf<RigidBody> id)
        {
            var vm = _presentersMap[id];
            vm.Remove();
            _presentersMap.Remove(id);

            var type = vm.Type;

            if (type == RemoteEntityType.Hero)
            {
                _totalHeroesSpawned--;
                RenderHeroesCount();
            }
        }

        public void Moved(IdentityOf<RigidBody> id, Vector newPosition)
        {
            var isPlayer = id == _playersRigidBodyId;
            if (isPlayer)
            {
                _lastCreaturePosition = newPosition;
            }

            if (!isPlayer && !_creaturesRenderingEnabled)
            {
                return;
            }

            RigidBodyPresenter presenter;
            if (!_presentersMap.TryGetValue(id, out presenter))
            {
                return;
            }

            presenter.SetNewPosition(newPosition);
        }

        public void RenderFps(double fps, long totalFrames)
        {
            var fpsString = fps.ToString(CultureInfo.InvariantCulture);
            _consoleRenderer.Render(0, 2, "       ");
            _consoleRenderer.Render(0, 2, fpsString);
        }

        public void RenderAverageEventsCount(double averageEventsCount, long totalEvents)
        {
            _consoleRenderer.Render(0, 0, totalEvents.ToString(CultureInfo.InvariantCulture));
            _consoleRenderer.Render(0, 1, "       ");
            _consoleRenderer.Render(0, 1, averageEventsCount.ToString(CultureInfo.InvariantCulture));
        }

        public void RenderCreatureState()
        {
            RenderPosition();
        }

        public void RenderCreatures()
        {
            if (!_creaturesRenderingEnabled)
            {
                return;
            }

            foreach (var vm in _presentersMap.Values)
            {
                vm.Render();
            }
        }

        private void SetViewPort()
        {
            _consoleRenderer = new ConsoleRenderer(_map.Dimensions.Width + 2, _map.Dimensions.Height + 2);
            _consoleRenderer.InitializeScreen();
        }

        private void RenderPosition()
        {
            var position = "p: " + _lastCreaturePosition.ToString("0.0");
            _consoleRenderer.Render(15, 0, "                ");
            _consoleRenderer.Render(15, 0, position);
        }

        private void RenderWorld()
        {
            var width = _map.Dimensions.Width;
            var height = _map.Dimensions.Height;

            _consoleRenderer.RenderHorizontalLine(0, 1, width);
            _consoleRenderer.RenderHorizontalLine(height + 1, 1, width);
            _consoleRenderer.RenderVerticalLine(1, 0, height + 1);
            _consoleRenderer.RenderVerticalLine(1, width + 1, height + 1);

            for (var y = 0; y < height; y++)
            {
                var chars = new char[width];
                for (var x = 0; x < width; x++)
                {
                    chars[x] = _map[x, y] == Tile.Solid ? 'W' : '·';
                }

                _consoleRenderer.Render(1, height - y, chars, ConsoleColor.DarkGray);
            }
        }

        private void RenderHeroesCount()
        {
            _consoleRenderer.Render(60, 0, "     ");
            _consoleRenderer.Render(60, 0, _totalHeroesSpawned.ToString(CultureInfo.InvariantCulture));
        }
    }
}