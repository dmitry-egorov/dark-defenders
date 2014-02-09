using System.IO;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Infrastructure;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Serialization;

namespace DarkDefenders.Domain.Serialization.Internals
{
    internal class SerializingReciever : IEventsReciever
    {
        private readonly BinaryWriter _writer;

        public SerializingReciever(BinaryWriter writer)
        {
            _writer = writer;
        }

        public void TerrainCreated(string mapId)
        {
            _writer.Write((short) SerializableEvents.TerrainCreated);
            _writer.Write(mapId);
        }

        public void RigidBodyCreated(IdentityOf<RigidBody> id, Vector position)
        {
            _writer.Write((short)SerializableEvents.RigidBodyCreated);
            _writer.Write(id);
            _writer.Write(position);
        }

        public void RigidBodyDestroyed(IdentityOf<RigidBody> id)
        {
            _writer.Write((short)SerializableEvents.RigidBodyDestroyed);
            _writer.Write(id);
        }

        public void Moved(IdentityOf<RigidBody> id, Vector newPosition)
        {
            _writer.Write((short)SerializableEvents.Moved);
            _writer.Write(id);
            _writer.Write(newPosition);
        }

        public void CreatureCreated(IdentityOf<Creature> id, IdentityOf<RigidBody> rigidBodyId)
        {
            _writer.Write((short)SerializableEvents.CreatureCreated);
            _writer.Write(id);
            _writer.Write(rigidBodyId);
        }

        public void HeroCreated(IdentityOf<Creature> creatureId)
        {
            _writer.Write((short)SerializableEvents.HeroCreated);
            _writer.Write(creatureId);
        }

        public void HeroDestroyed()
        {
            _writer.Write((short)SerializableEvents.HeroDestroyed);
        }

        public void PlayerAvatarSpawned(IdentityOf<Creature> creatureId)
        {
            _writer.Write((short)SerializableEvents.PlayerAvatarSpawned);
            _writer.Write(creatureId);
        }

        public void ProjectileCreated(IdentityOf<RigidBody> rigidBodyId)
        {
            _writer.Write((short)SerializableEvents.ProjectileCreated);
            _writer.Write(rigidBodyId);
        }
    }
}