﻿using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Creatures.Events;
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
        private ConsoleRenderer _consoleRenderer;

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

        public void Recieve(CreatureCreated creatureCreated)
        {
            _creatureRigidBodyId = creatureCreated.RigidBodyId;
            var vm = _rigidBodyMap[creatureCreated.RigidBodyId];
            vm.SetAsCreature();
        }

        public void Recieve(MovementChanged movementChanged)
        {
            RenderMovementForce(movementChanged);
        }

        public void Recieve(CreatureFired creatureFired)
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
            var creatureViewModel = new RigidBodyViewModel(_map, _consoleRenderer);
            _rigidBodyMap.Add(rigidBodyCreated.RootId, creatureViewModel);

            creatureViewModel.Recieve(rigidBodyCreated);
        }

        public void Recieve(RigidBodyDestroyed rigidBodyDestroyed)
        {
            var vm = _rigidBodyMap[rigidBodyDestroyed.RootId];
            vm.Remove();
            _rigidBodyMap.Remove(rigidBodyDestroyed.RootId);
        }

        public void Recieve(CreatureDestroyed creatureDestroyed)
        {
            throw new System.NotImplementedException();
        }

        public void Recieve(Moved moved)
        {
            if (moved.RootId == _creatureRigidBodyId)
            {
                _lastCreaturePosition = moved.NewPosition;
            }

            var viewModel = _rigidBodyMap[moved.RootId];
            viewModel.Recieve(moved);
        }

        public void Recieve(Accelerated creatureAccelerated)
        {
            if (creatureAccelerated.RootId == _creatureRigidBodyId)
            {
                _lastCreatureMomentum = creatureAccelerated.NewMomentum;
            }
        }

        public void Recieve(ExternalForceChanged externalForceChanged)
        {
        }

        public void RenderFps(double fps, long totalFrames)
        {
            var fpsString = fps.ToString(CultureInfo.InvariantCulture);
            _consoleRenderer.RenderFloatRight(fpsString, 0, 8, _map.Dimensions.Width + 2);
        }

        public void RenderAverageEventsCount(double averageEventsCount, long totalEvents)
        {
            _consoleRenderer.Render(0, 1, "      ");
            _consoleRenderer.Render(0, 1, averageEventsCount.ToString(CultureInfo.InvariantCulture));
            _consoleRenderer.Render(0, 0, totalEvents.ToString(CultureInfo.InvariantCulture));
        }

        public void RenderCreatureState()
        {
            RenderPosition();
            RenderMomentum();
        }

        private void SetViewPort()
        {
            _consoleRenderer = new ConsoleRenderer(_map.Dimensions.Width + 2, _map.Dimensions.Height + 2);
            _consoleRenderer.InitializeScreen();
        }

        private void RenderMovementForce(MovementChanged movementChanged)
        {
            var text = "d: " + movementChanged.Movement;
            _consoleRenderer.RenderFloatRight(text, 3, 8, _map.Dimensions.Width + 2);
        }

        private void RenderPosition()
        {
            _consoleRenderer.RenderFloatRight("p: " + _lastCreaturePosition.ToString("0.0"), 1, 15, _map.Dimensions.Width + 2);
        }

        private void RenderMomentum()
        {
            _consoleRenderer.RenderFloatRight("v: " + _lastCreatureMomentum.ToString("0.0"), 2, 18, _map.Dimensions.Width + 2);
        }

        private void RenderWorld()
        {
            var width = _map.Dimensions.Width;
            var height = _map.Dimensions.Height;

            _consoleRenderer.RenderHorizontalLine(0, 1, width);
            _consoleRenderer.RenderHorizontalLine(height + 1, 1, width);
            _consoleRenderer.RenderVerticalLine(1, 0, height + 1);
            _consoleRenderer.RenderVerticalLine(1, width + 1, height + 1);

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var c = _map[i, j] == Tile.Solid ? 'W' : '·';

                    _consoleRenderer.Render(new Point(i + 1, height - j), c);
                }
            }
        }

        private readonly Dictionary<RigidBodyId, RigidBodyViewModel> _rigidBodyMap = new Dictionary<RigidBodyId, RigidBodyViewModel>();

        private Vector _lastCreatureMomentum = Vector.Zero;
        private Vector _lastCreaturePosition = Vector.Zero;
        private RigidBodyId _creatureRigidBodyId;
        private Map<Tile> _map;
    }
}