using System;
using System.IO;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Domain.Serialization.Internals;
using Infrastructure.Serialization;

namespace DarkDefenders.Domain.Serialization
{
    public class EventsDeserializer
    {
        private readonly IEventsReciever _reciever;

        public EventsDeserializer(IEventsReciever reciever)
        {
            _reciever = reciever;
        }

        public void Deserialize(byte[] eventData)
        {
            eventData.UsingBinaryReader(reader =>
            {
                while (reader.PeekChar() != -1)
                {
                    var eventType = (SerializableEvents)reader.ReadInt16();

                    switch (eventType)
                    {
                        case SerializableEvents.TerrainCreated:
                            ReadTerrainCreated(reader);
                            break;
                        case SerializableEvents.RigidBodyCreated:
                            ReadRigidBodyCreated(reader);
                            break;
                        case SerializableEvents.RigidBodyDestroyed:
                            ReadRigidBodyDestroyed(reader);
                            break;
                        case SerializableEvents.Moved:
                            ReadMoved(reader);
                            break;
                        case SerializableEvents.CreatureCreated:
                            ReadCreatureCreated(reader);
                            break;
                        case SerializableEvents.HeroCreated:
                            ReadHeroCreated(reader);
                            break;
                        case SerializableEvents.HeroDestroyed:
                            ReadHeroDestroyed(reader);
                            break;
                        case SerializableEvents.PlayerAvatarSpawned:
                            ReadPlayerAvatarSpawned(reader);
                            break;
                        case SerializableEvents.ProjectileCreated:
                            ReadProjectileCreated(reader);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            });
        }

        private void ReadTerrainCreated(BinaryReader reader)
        {
            var mapId = reader.ReadString();
            _reciever.TerrainCreated(mapId);
        }

        private void ReadRigidBodyCreated(BinaryReader reader)
        {
            var id = reader.ReadIdentityOf<RigidBody>();
            var position = reader.ReadVector();

            _reciever.RigidBodyCreated(id, position);
        }

        private void ReadRigidBodyDestroyed(BinaryReader reader)
        {
            var id = reader.ReadIdentityOf<RigidBody>();
            _reciever.RigidBodyDestroyed(id);
        }

        private void ReadMoved(BinaryReader reader)
        {
            var id = reader.ReadIdentityOf<RigidBody>();
            var newPosition = reader.ReadVector();

            _reciever.Moved(id, newPosition);
        }

        private void ReadCreatureCreated(BinaryReader reader)
        {
            var id = reader.ReadIdentityOf<Creature>();
            var rigidBodyId = reader.ReadIdentityOf<RigidBody>();

            _reciever.CreatureCreated(id, rigidBodyId);
        }

        private void ReadHeroCreated(BinaryReader reader)
        {
            var creatureId = reader.ReadIdentityOf<Creature>();

            _reciever.HeroCreated(creatureId);
        }

        private void ReadHeroDestroyed(BinaryReader reader)
        {
            _reciever.HeroDestroyed();
        }

        private void ReadPlayerAvatarSpawned(BinaryReader reader)
        {
            var creatureId = reader.ReadIdentityOf<Creature>();

            _reciever.PlayerAvatarSpawned(creatureId);
        }

        private void ReadProjectileCreated(BinaryReader reader)
        {
            var rigidBodyId = reader.ReadIdentityOf<RigidBody>();

            _reciever.ProjectileCreated(rigidBodyId);
        }
    }
}