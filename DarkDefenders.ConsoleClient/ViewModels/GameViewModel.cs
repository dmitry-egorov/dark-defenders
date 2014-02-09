using System;
using System.Collections.Generic;
using System.Globalization;
using DarkDefenders.ConsoleClient.Renderer;
using DarkDefenders.Domain.Data.Entities.Clocks;
using DarkDefenders.Domain.Data.Entities.Creatures;
using DarkDefenders.Domain.Data.Entities.Heroes;
using DarkDefenders.Domain.Data.Entities.Projectiles;
using DarkDefenders.Domain.Data.Entities.RigidBodies;
using DarkDefenders.Domain.Data.Entities.Terrains;
using DarkDefenders.Domain.Data.Entities.Worlds;
using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Data.Other;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Files;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.ConsoleClient.ViewModels
{
    internal class GameViewModel : IEventsListener<EventDataBase>, IEventDataReciever
    {
        private IConsoleRenderer _consoleRenderer;
        private readonly Dictionary<IdentityOf<RigidBody>, RigidBodyViewModel> _viewModelsMap = new Dictionary<IdentityOf<RigidBody>, RigidBodyViewModel>();
        private readonly Dictionary<IdentityOf<Creature>, IdentityOf<RigidBody>> _rigidBodyIdsMap = new Dictionary<IdentityOf<Creature>, IdentityOf<RigidBody>>();

        private Momentum _lastCreatureMomentum = Momentum.Zero;
        private Vector _lastCreaturePosition = Vector.Zero;
        private IdentityOf<RigidBody> _playersRigidBodyId;
        private Map<Tile> _map;
        private int _totalHeroesSpawned;
        private bool _creaturesRenderingEnabled = true;

        public GameViewModel()
        {
            _consoleRenderer = new EmptyConsoleRenderer();
        }

        public void Recieve(EventDataBase entityEvent)
        {
            entityEvent.Accept(this);
        }

        public void Recieve(ClockCreatedData clockCreated)
        {
            
        }

        public void Recieve(WorldCreatedData worldCreated)
        {
            
        }

        public void Recieve(TerrainCreatedData terrainCreated)
        {
            var data = TerrainLoader.LoadFromFile(terrainCreated.MapId);

            _map = data.Map;

            SetViewPort();
            RenderWorld();
        }

        public void Recieve(TimeChangedData clockTimeUpdated)
        {

        }

        public void Recieve(HeroSpawnActivationTimeChangedData heroSpawnActivationTimeChanged)
        {
            
        }

        public void Recieve(SpawnHeroesChangedData spawnHeroesChangedData)
        {
            
        }

        public void Recieve(PlayerAvatarSpawnedData playerAvatarSpawned)
        {
            var rigidBodyId = _rigidBodyIdsMap[playerAvatarSpawned.CreatureId];

            var vm = _viewModelsMap[rigidBodyId];

            vm.SetAsPlayersAvatar();

            _playersRigidBodyId = rigidBodyId;
        }

        public void Recieve(CreatureCreatedData creatureCreated)
        {
            _rigidBodyIdsMap[creatureCreated.CreatureId] = creatureCreated.RigidBodyId;
        }

        public void Recieve(MovementChangedData movementChanged)
        {

        }
        
        public void Recieve(FiredData fired)
        {
            
        }

        public void Recieve(ProjectileCreatedData projectileCreated)
        {
            var vm = _viewModelsMap[projectileCreated.RigidBodyId];
            vm.SetAsProjectile();
        }

        public void Recieve(ProjectileDestroyedData projectileDestroyed)
        {
        }

        public void Recieve(HeroCreatedData heroCreated)
        {
            var rigidBodyId = _rigidBodyIdsMap[heroCreated.CreatureId];

            var vm = _viewModelsMap[rigidBodyId];

            vm.SetAsHero();

            _totalHeroesSpawned++;
            RenderHeroesCount();
        }

        public void Recieve(HeroDestroyedData heroDestroyedData)
        {
            _totalHeroesSpawned--;
            RenderHeroesCount();
        }

        public void Recieve(RigidBodyCreatedData rigidBodyCreated)
        {
            var creatureViewModel = new RigidBodyViewModel(_map, _consoleRenderer);
            _viewModelsMap.Add(rigidBodyCreated.RigidBodyId, creatureViewModel);

            creatureViewModel.Recieve(rigidBodyCreated);
        }

        public void Recieve(RigidBodyDestroyedData rigidBodyDestroyed)
        {
            var rigidBodyId = rigidBodyDestroyed.RigidBodyId;
            var vm = _viewModelsMap[rigidBodyId];
            vm.Remove();
            _viewModelsMap.Remove(rigidBodyId);
        }

        public void Recieve(CreatureDestroyedData creatureDestroyed)
        {

        }

        public void Recieve(MovedData moved)
        {
            var rigidBodyId = moved.RigidBodyId;
            var newPosition = moved.NewPosition.ToVector();

            SetNewPosition(rigidBodyId, newPosition);
        }

        public void Recieve(AcceleratedData creatureAccelerated)
        {
            var rigidBodyId = creatureAccelerated.RigidBodyId;
            var newMomentum = creatureAccelerated.NewMomentum.ToMomentum();

            SetNewMomentum(rigidBodyId, newMomentum);
        }

        public void Recieve(AcceleratedAndMovedData acceleratedAndMoved)
        {
            var rigidBodyId = acceleratedAndMoved.RigidBodyId;
            SetNewMomentum(rigidBodyId, acceleratedAndMoved.NewMomentum.ToMomentum());
            SetNewPosition(rigidBodyId, acceleratedAndMoved.NewPosition.ToVector());
        }

        public void Recieve(ExternalForceChangedData externalForceChanged)
        {
        }

        public void Recieve(StateChangedData stateChanged)
        {
            
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
            RenderMomentum();
        }

        public void RenderCreatures()
        {
            if (!_creaturesRenderingEnabled)
            {
                return;
            }

            foreach (var vm in _viewModelsMap.Values)
            {
                vm.Render();
            }
        }

        private void SetNewPosition(IdentityOf<RigidBody> rigidBodyId, Vector newPosition)
        {
            var isPlayer = rigidBodyId == _playersRigidBodyId;
            if (isPlayer)
            {
                _lastCreaturePosition = newPosition;
            }

            if (!isPlayer && !_creaturesRenderingEnabled)
            {
                return;
            }

            var viewModel = _viewModelsMap[rigidBodyId];

            viewModel.SetNewPosition(newPosition);
        }

        private void SetNewMomentum(IdentityOf<RigidBody> rigidBodyId, Momentum newMomentum)
        {
            if (rigidBodyId == _playersRigidBodyId)
            {
                _lastCreatureMomentum = newMomentum;
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

        private void RenderMomentum()
        {
            var text = "v: " + _lastCreatureMomentum.Value.ToString("0.0");
            _consoleRenderer.Render(35, 0, "                  ");
            _consoleRenderer.Render(35, 0, text);
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