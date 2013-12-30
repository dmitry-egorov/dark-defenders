using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.Projectiles.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Console.ViewModels
{
    internal class GameViewModel : IEventsListener<IDomainEvent>, IDomainEventReciever
    {
        public void Recieve(IDomainEvent domainEvent)
        {
            domainEvent.Accept(this);
        }

        public void Recieve(WorldCreated worldCreated)
        {
            _map = worldCreated.Map;

            SetViewPort();
            RenderWorld();
        }

        public void Recieve(WorldDestroyed worldDestroyed)
        {
            throw new System.NotImplementedException();
        }

        public void Recieve(WorldTimeUpdated worldTimeUpdated)
        {

        }

        public void Recieve(PlayerCreated playerCreated)
        {
            _playerRigidBodyId = playerCreated.RigidBodyId;
            var vm = _rigidBodyMap[playerCreated.RigidBodyId];
            vm.SetAsPlayer();
        }

        public void Recieve(MovementForceDirectionChanged movementForceDirectionChanged)
        {
            RenderMovementForce(movementForceDirectionChanged);
        }

        public void Recieve(PlayerFired playerFired)
        {
            
        }

        public void Recieve(ProjectileCreated projectileCreated)
        {
            var vm = _rigidBodyMap[projectileCreated.RigidBodyId];
            vm.SetAsProjectile();
        }

        public void Recieve(ProjectileDestroyed projectileCreated)
        {
        }

        public void Recieve(RigidBodyCreated rigidBodyCreated)
        {
            var playerViewModel = new RigidBodyViewModel(_map);
            _rigidBodyMap.Add(rigidBodyCreated.RootId, playerViewModel);

            playerViewModel.Recieve(rigidBodyCreated);
        }

        public void Recieve(RigidBodyDestroyed rigidBodyDestroyed)
        {
            var vm = _rigidBodyMap[rigidBodyDestroyed.RootId];
            vm.Remove();
            _rigidBodyMap.Remove(rigidBodyDestroyed.RootId);
        }

        public void Recieve(PlayerDestroyed playerDestroyed)
        {
            throw new System.NotImplementedException();
        }

        public void Recieve(Moved moved)
        {
            if (moved.RootId == _playerRigidBodyId)
            {
                _lastPlayerPosition = moved.NewPosition;
            }

            var viewModel = _rigidBodyMap[moved.RootId];
            viewModel.Recieve(moved);
        }

        public void Recieve(Accelerated playerAccelerated)
        {
            if (playerAccelerated.RootId == _playerRigidBodyId)
            {
                _lastPlayerMomentum = playerAccelerated.NewMomentum;
            }
        }

        public void Recieve(ExternalForceChanged externalForceChanged)
        {
        }

        public void RenderFps(double fps, long totalFrames)
        {
            var fpsString = fps.ToString(CultureInfo.InvariantCulture);
            ConsoleRenderer.RenderFloatRight(fpsString, 0, 8, _map.Dimensions.Width + 2);
        }

        public void RenderAverageEventsCount(double averageEventsCount, long totalEvents)
        {
            ConsoleRenderer.Render(0, 1, "     ");
            ConsoleRenderer.Render(0, 1, averageEventsCount.ToString(CultureInfo.InvariantCulture));
            ConsoleRenderer.Render(0, 0, totalEvents.ToString(CultureInfo.InvariantCulture));
        }

        public void RenderPlayerState()
        {
            RenderPosition();
            RenderMomentum();
        }

        private void SetViewPort()
        {
            ConsoleRenderer.SetViewPort(_map.Dimensions.Width + 2, _map.Dimensions.Height + 2);
        }

        private void RenderMovementForce(MovementForceDirectionChanged movementForceDirectionChanged)
        {
            var text = "d: " + movementForceDirectionChanged.MovementForceDirection;
            ConsoleRenderer.RenderFloatRight(text, 3, 30, _map.Dimensions.Width + 2);
        }

        private void RenderPosition()
        {
            ConsoleRenderer.RenderFloatRight("p: " + _lastPlayerPosition.ToString("0.0"), 1, 30, _map.Dimensions.Width + 2);
        }

        private void RenderMomentum()
        {
            ConsoleRenderer.RenderFloatRight("v: " + _lastPlayerMomentum.ToString("0.0"), 2, 30, _map.Dimensions.Width + 2);
        }

        private void RenderWorld()
        {
            var width = _map.Dimensions.Width;
            var height = _map.Dimensions.Height;

            ConsoleRenderer.RenderHorizontalLine(0, 1, width);
            ConsoleRenderer.RenderHorizontalLine(height + 1, 1, width);
            ConsoleRenderer.RenderVerticalLine(1, 0, height + 1);
            ConsoleRenderer.RenderVerticalLine(1, width + 1, height + 1);

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    if (_map[i, j] == 1)
                    {
                        ConsoleRenderer.Render(new Point(i + 1, height - j), '+');
                    }
                }
            }
        }

        private readonly Dictionary<RigidBodyId, RigidBodyViewModel> _rigidBodyMap = new Dictionary<RigidBodyId, RigidBodyViewModel>();

        private Vector _lastPlayerMomentum = Vector.Zero;
        private Vector _lastPlayerPosition = Vector.Zero;
        private RigidBodyId _playerRigidBodyId;
        private Map _map;
    }
}