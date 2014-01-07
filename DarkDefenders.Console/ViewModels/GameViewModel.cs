using System;
using System.Collections.Generic;
using System.Globalization;
using DarkDefenders.Domain.Clocks.Events;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Heroes;
using DarkDefenders.Domain.Heroes.Events;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Creatures.Events;
using DarkDefenders.Domain.Projectiles.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Terrains.Events;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Console.ViewModels
{
    internal class GameViewModel : IEventsListener<IDomainEvent>, IDomainEventsReciever
    {
        public void Recieve(IDomainEvent domainEvent)
        {
            domainEvent.ApplyTo(this);
        }

        public void Recieve(ClockCreated clockCreated)
        {
            
        }

        public void Recieve(ClockDestroyed clockDestroyed)
        {
            throw new NotImplementedException();
        }

        public void Recieve(WorldCreated worldCreated)
        {
            
        }

        public void Recieve(TerrainCreated terrainCreated)
        {
            _map = terrainCreated.Map;

            SetViewPort();
            RenderWorld();
        }

        public void Recieve(TerrainDestroyed terrainDestroyed)
        {
            throw new NotImplementedException();
        }

        public void Recieve(WorldDestroyed worldDestroyed)
        {
            throw new NotImplementedException();
        }

        public void Recieve(ClockTimeUpdated clockTimeUpdated)
        {

        }

        public void Recieve(HeroSpawned heroSpawned)
        {
            
        }

        public void Recieve(PlayerAvatarSpawned playerAvatarSpawned)
        {
            var rigidBodyId = _rigidBodyIdsMap[playerAvatarSpawned.CreatureId];

            var vm = _viewModelsMap[rigidBodyId];

            vm.SetAsPlayersAvatar();

            _playersRigidBodyId = rigidBodyId;
        }

        public void Recieve(CreatureCreated creatureCreated)
        {
            _rigidBodyIdsMap[creatureCreated.RootId] = creatureCreated.RigidBodyId;
        }

        public void Recieve(MovementChanged movementChanged)
        {
            //RenderMovementForce(movementChanged);
        }

        public void Recieve(CreatureFired creatureFired)
        {
            
        }

        public void Recieve(ProjectileCreated projectileCreated)
        {
            var vm = _viewModelsMap[projectileCreated.RigidBodyId];
            vm.SetAsProjectile();
        }

        public void Recieve(ProjectileDestroyed projectileCreated)
        {
        }

        public void Recieve(HeroCreated heroCreated)
        {
            var rigidBodyId = _rigidBodyIdsMap[heroCreated.CreatureId];

            var vm = _viewModelsMap[rigidBodyId];

            vm.SetAsHero();

            _totalHeroesSpawned++;
            RenderHeroesCount();
        }

        public void Recieve(HeroDestroyed heroDestroyed)
        {
            _totalHeroesSpawned--;
            RenderHeroesCount();
        }

        public void Recieve(RigidBodyCreated rigidBodyCreated)
        {
            var creatureViewModel = new RigidBodyViewModel(_map, _consoleRenderer);
            _viewModelsMap.Add(rigidBodyCreated.RootId, creatureViewModel);

            creatureViewModel.Recieve(rigidBodyCreated);
        }

        public void Recieve(RigidBodyDestroyed rigidBodyDestroyed)
        {
            var vm = _viewModelsMap[rigidBodyDestroyed.RootId];
            vm.Remove();
            _viewModelsMap.Remove(rigidBodyDestroyed.RootId);
        }

        public void Recieve(CreatureDestroyed creatureDestroyed)
        {

        }

        public void Recieve(Moved moved)
        {
            var rigidBodyId = moved.RootId;
            var newPosition = moved.NewPosition;

            SetNewPosition(rigidBodyId, newPosition);
        }

        private void SetNewPosition(RigidBodyId rigidBodyId, Vector newPosition)
        {
            if (rigidBodyId == _playersRigidBodyId)
            {
                _lastCreaturePosition = newPosition;
            }

            var viewModel = _viewModelsMap[rigidBodyId];

            viewModel.SetNewPosition(newPosition);
        }

        public void Recieve(Accelerated creatureAccelerated)
        {
            var rigidBodyId = creatureAccelerated.RootId;
            var newMomentum = creatureAccelerated.NewMomentum;

            SetNewMomentum(rigidBodyId, newMomentum);
        }

        private void SetNewMomentum(RigidBodyId rigidBodyId, Momentum newMomentum)
        {
            if (rigidBodyId == _playersRigidBodyId)
            {
                _lastCreatureMomentum = newMomentum;
            }
        }

        public void Recieve(AcceleratedAndMoved acceleratedAndMoved)
        {
            SetNewMomentum(acceleratedAndMoved.RootId, acceleratedAndMoved.NewMomentum);
            SetNewPosition(acceleratedAndMoved.RootId, acceleratedAndMoved.NewPosition);
        }

        public void Recieve(ExternalForceChanged externalForceChanged)
        {
        }

        public void Recieve(StateChanged stateChanged)
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
            foreach (var vm in _viewModelsMap.Values)
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

        private ConsoleRenderer _consoleRenderer;
        private readonly Dictionary<RigidBodyId, RigidBodyViewModel> _viewModelsMap = new Dictionary<RigidBodyId, RigidBodyViewModel>();
        private readonly Dictionary<CreatureId, RigidBodyId> _rigidBodyIdsMap = new Dictionary<CreatureId, RigidBodyId>();

        private Momentum _lastCreatureMomentum = Momentum.Zero;
        private Vector _lastCreaturePosition = Vector.Zero;
        private RigidBodyId _playersRigidBodyId;
        private Map<Tile> _map;
        private int _totalHeroesSpawned;
    }
}