using System;
using System.Collections.Generic;
using System.Globalization;
using DarkDefenders.Dtos.Entities.Clocks;
using DarkDefenders.Dtos.Entities.Creatures;
using DarkDefenders.Dtos.Entities.Heroes;
using DarkDefenders.Dtos.Entities.Projectiles;
using DarkDefenders.Dtos.Entities.RigidBodies;
using DarkDefenders.Dtos.Entities.Terrains;
using DarkDefenders.Dtos.Entities.Worlds;
using DarkDefenders.Dtos.Infrastructure;
using DarkDefenders.Dtos.Other;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Console.ViewModels
{
    internal class GameViewModel : IEventsListener<IEventDto>, IEventDtoReciever
    {
        private ConsoleRenderer _consoleRenderer;
        private readonly Dictionary<RigidBodyId, RigidBodyViewModel> _viewModelsMap = new Dictionary<RigidBodyId, RigidBodyViewModel>();
        private readonly Dictionary<CreatureId, RigidBodyId> _rigidBodyIdsMap = new Dictionary<CreatureId, RigidBodyId>();

        private Momentum _lastCreatureMomentum = Momentum.Zero;
        private Vector _lastCreaturePosition = Vector.Zero;
        private RigidBodyId _playersRigidBodyId;
        private Map<Tile> _map;
        private int _totalHeroesSpawned;
        private bool _creaturesRenderingEnabled = true;

        public void Recieve(IEventDto entityEvent)
        {
            entityEvent.Accept(this);
        }

        public void Recieve(ClockCreatedDto clockCreated)
        {
            
        }

        public void Recieve(WorldCreatedDto worldCreated)
        {
            
        }

        public void Recieve(TerrainCreatedDto terrainCreated)
        {
            _map = terrainCreated.Map;

            SetViewPort();
            RenderWorld();
        }

        public void Recieve(TimeChangedDto clockTimeUpdated)
        {

        }

        public void Recieve(HeroSpawnActivationTimeChangedDto heroSpawnActivationTimeChanged)
        {
            
        }

        public void Recieve(SpawnHeroesChangedDto spawnHeroesChangedDto)
        {
            
        }

        public void Recieve(PlayerAvatarSpawnedDto playerAvatarSpawned)
        {
            var rigidBodyId = _rigidBodyIdsMap[playerAvatarSpawned.CreatureId];

            var vm = _viewModelsMap[rigidBodyId];

            vm.SetAsPlayersAvatar();

            _playersRigidBodyId = rigidBodyId;
        }

        public void Recieve(CreatureCreatedDto creatureCreated)
        {
            _rigidBodyIdsMap[creatureCreated.CreatureId] = creatureCreated.RigidBodyId;
        }

        public void Recieve(MovementChangedDto movementChanged)
        {

        }
        
        public void Recieve(FiredDto fired)
        {
            
        }

        public void Recieve(ProjectileCreatedDto projectileCreated)
        {
            var vm = _viewModelsMap[projectileCreated.RigidBodyId];
            vm.SetAsProjectile();
        }

        public void Recieve(ProjectileDestroyedDto projectileDestroyed)
        {
        }

        public void Recieve(HeroCreatedDto heroCreated)
        {
            var rigidBodyId = _rigidBodyIdsMap[heroCreated.CreatureId];

            var vm = _viewModelsMap[rigidBodyId];

            vm.SetAsHero();

            _totalHeroesSpawned++;
            RenderHeroesCount();
        }

        public void Recieve(HeroDestroyedDto heroDestroyedDto)
        {
            _totalHeroesSpawned--;
            RenderHeroesCount();
        }

        public void Recieve(RigidBodyCreatedDto rigidBodyCreated)
        {
            var creatureViewModel = new RigidBodyViewModel(_map, _consoleRenderer);
            _viewModelsMap.Add(rigidBodyCreated.RigidBodyId, creatureViewModel);

            creatureViewModel.Recieve(rigidBodyCreated);
        }

        public void Recieve(RigidBodyDestroyedDto rigidBodyDestroyed)
        {
            var vm = _viewModelsMap[rigidBodyDestroyed.RigidBodyId];
            vm.Remove();
            _viewModelsMap.Remove(rigidBodyDestroyed.RigidBodyId);
        }

        public void Recieve(CreatureDestroyedDto creatureDestroyed)
        {

        }

        public void Recieve(MovedDto moved)
        {
            var rigidBodyId = moved.RigidBodyId;
            var newPosition = moved.NewPosition;

            SetNewPosition(rigidBodyId, newPosition);
        }

        public void Recieve(AcceleratedDto creatureAccelerated)
        {
            var rigidBodyId = creatureAccelerated.RigidBodyId;
            var newMomentum = creatureAccelerated.NewMomentum;

            SetNewMomentum(rigidBodyId, newMomentum);
        }

        public void Recieve(AcceleratedAndMovedDto acceleratedAndMoved)
        {
            SetNewMomentum(acceleratedAndMoved.RigidBodyId, acceleratedAndMoved.NewMomentum);
            SetNewPosition(acceleratedAndMoved.RigidBodyId, acceleratedAndMoved.NewPosition);
        }

        public void Recieve(ExternalForceChangedDto externalForceChanged)
        {
        }

        public void Recieve(StateChangedDto stateChanged)
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

        private void SetNewPosition(RigidBodyId rigidBodyId, Vector newPosition)
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

        private void SetNewMomentum(RigidBodyId rigidBodyId, Momentum newMomentum)
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