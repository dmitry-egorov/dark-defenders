using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Infrastructure;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.ConsoleClient
{
    public class CountingEventsListener : IEventsReciever
    {
        private long _totalCount;
        private long _lastCount;

        public long TotalCount { get { return _totalCount; } }

        public int EventsSinceLastCall
        {
            get
            {
                var count = TotalCount - _lastCount;
                _lastCount = TotalCount;

                return (int)count;
            }
        }

        public void TerrainCreated(string mapId)
        {
            _totalCount += 1;
        }

        public void RigidBodyCreated(IdentityOf<RigidBody> id, Vector position)
        {
            _totalCount += 1;
        }

        public void RigidBodyDestroyed(IdentityOf<RigidBody> id)
        {
            _totalCount += 1;
        }

        public void Moved(IdentityOf<RigidBody> id, Vector newPosition)
        {
            _totalCount += 1;
        }

        public void CreatureCreated(IdentityOf<Creature> id, IdentityOf<RigidBody> rigidBodyId)
        {
            _totalCount += 1;
        }

        public void HeroCreated(IdentityOf<Creature> creatureId)
        {
            _totalCount += 1;
        }

        public void HeroDestroyed()
        {
            _totalCount += 1;
        }

        public void PlayerAvatarSpawned(IdentityOf<Creature> creatureId)
        {
            _totalCount += 1;
        }

        public void ProjectileCreated(IdentityOf<RigidBody> rigidBodyId)
        {
            _totalCount += 1;
        }
    }
}