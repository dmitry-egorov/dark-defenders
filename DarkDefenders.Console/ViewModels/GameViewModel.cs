using System.Collections.Generic;
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
    internal class GameViewModel : IEventsLinstener<IDomainEvent>, IDomainEventReciever
    {
        public static GameViewModel InitializeNew()
        {
            var renderer = new GameViewModel(100, 40);

            renderer.Initialize();

            return renderer;
        }

        public GameViewModel(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void Initialize()
        {
            System.Console.BufferWidth = System.Console.WindowWidth = _width;
            System.Console.BufferHeight = System.Console.WindowHeight = _height;

            System.Console.CursorVisible = false;
        }

        public void Recieve(IEnumerable<IDomainEvent> events)
        {
            foreach (var domainEvent in events)
            {
                domainEvent.Accept(this);
            }
        }

        public void Recieve(WorldCreated worldCreated)
        {
            RenderWorld(_width, _height);
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
            var playerViewModel = new RigidBodyViewModel(_width, _height);
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
                _lastPosition = moved.NewPosition;
            }

            var viewModel = _rigidBodyMap[moved.RootId];
            viewModel.Recieve(moved);
        }

        public void Recieve(Accelerated playerAccelerated)
        {
            if (playerAccelerated.RootId == _playerRigidBodyId)
            {
                _lastMomentum = playerAccelerated.NewMomentum;
            }
        }

        public void Recieve(ExternalForceChanged externalForceChanged)
        {
        }

        public void RenderFps(double fps, long totalFrames)
        {
            var fpsString = fps.ToString(CultureInfo.InvariantCulture);
            ConsoleRenderer.RenderFloatRight(fpsString, 0, 8, _width);
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

        private void RenderMovementForce(MovementForceDirectionChanged movementForceDirectionChanged)
        {
            var text = "d: " + movementForceDirectionChanged.MovementForceDirection;
            ConsoleRenderer.RenderFloatRight(text, 3, 30, _width);
        }

        private void RenderPosition()
        {
            ConsoleRenderer.RenderFloatRight("p: " + _lastPosition.ToString("0.00"), 1, 30, _width);
        }

        private void RenderMomentum()
        {
            ConsoleRenderer.RenderFloatRight("v: " + _lastMomentum.ToString("0.00"), 2, 30, _width);
        }

        private void RenderWorld(int width, int height)
        {
            ConsoleRenderer.RenderHorizontalLine(0, 1, width - 2);
            ConsoleRenderer.RenderHorizontalLine(height - 1, 1, width - 2);
            ConsoleRenderer.RenderVerticalLine(1, 0, height - 1);
            ConsoleRenderer.RenderVerticalLine(1, width - 1, height - 1);
        }

        private readonly Dictionary<RigidBodyId, RigidBodyViewModel> _rigidBodyMap = new Dictionary<RigidBodyId, RigidBodyViewModel>();

        private readonly int _width;
        private readonly int _height;
        private Vector _lastMomentum = Vector.Zero;
        private Vector _lastPosition = Vector.Zero;
        private RigidBodyId _playerRigidBodyId;
    }
}