using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Infrastructure;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.ConsoleClient
{
    public class CompositeEventsListener : IEventsReciever
    {
        private readonly ReadOnlyCollection<IEventsReciever> _listeners;

        public CompositeEventsListener(params IEventsReciever[] listeners)
            : this(listeners.AsEnumerable())
        {
            
        }

        public CompositeEventsListener(IEnumerable<IEventsReciever> linsteners)
        {
            _listeners = linsteners.ShouldNotBeNull("listeners").AsReadOnly();
        }

        public void TerrainCreated(string mapId)
        {
            foreach (var linstener in _listeners)
            {
                linstener.TerrainCreated(mapId);
            }
        }

        public void RigidBodyCreated(IdentityOf<RigidBody> id, Vector position)
        {
            foreach (var linstener in _listeners)
            {
                linstener.RigidBodyCreated(id, position);
            }
        }

        public void RigidBodyDestroyed(IdentityOf<RigidBody> id)
        {
            foreach (var linstener in _listeners)
            {
                linstener.RigidBodyDestroyed(id);
            }
        }

        public void Moved(IdentityOf<RigidBody> id, Vector newPosition)
        {
            foreach (var linstener in _listeners)
            {
                linstener.Moved(id, newPosition);
            }
        }

        public void CreatureCreated(IdentityOf<Creature> id, IdentityOf<RigidBody> rigidBodyId)
        {
            foreach (var linstener in _listeners)
            {
                linstener.CreatureCreated(id, rigidBodyId);
            }
        }

        public void HeroCreated(IdentityOf<Creature> creatureId)
        {
            foreach (var linstener in _listeners)
            {
                linstener.HeroCreated(creatureId);
            }
        }

        public void HeroDestroyed()
        {
            foreach (var linstener in _listeners)
            {
                linstener.HeroDestroyed();
            }
        }

        public void PlayerAvatarSpawned(IdentityOf<Creature> creatureId)
        {
            foreach (var linstener in _listeners)
            {
                linstener.PlayerAvatarSpawned(creatureId);
            };
        }

        public void ProjectileCreated(IdentityOf<RigidBody> rigidBodyId)
        {
            foreach (var linstener in _listeners)
            {
                linstener.ProjectileCreated(rigidBodyId);
            }
        }
    }
}