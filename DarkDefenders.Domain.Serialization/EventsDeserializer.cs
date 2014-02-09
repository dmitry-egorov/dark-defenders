using System;
using System.Collections.Generic;
using System.IO;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Interfaces;
using DarkDefenders.Domain.Serialization.Internals;
using Infrastructure.Serialization;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Serialization
{
    public class EventsDeserializer
    {
        public IEnumerable<Action<IEventsReciever>> Deserialize(byte[] eventData)
        {
            return eventData.UsingBinaryReader(reader => Read(reader).AsReadOnly());
        }

        private static IEnumerable<Action<IEventsReciever>> Read(BinaryReader reader)
        {
            while (reader.PeekChar() != -1)
            {
                var eventType = (SerializableEvents) reader.ReadInt16();

                switch (eventType)
                {
                    case SerializableEvents.TerrainCreated:
                        yield return ReadTerrainCreated(reader);
                        break;
                    case SerializableEvents.RigidBodyCreated:
                        yield return ReadRigidBodyCreated(reader);
                        break;
                    case SerializableEvents.RigidBodyDestroyed:
                        yield return ReadRigidBodyDestroyed(reader);
                        break;
                    case SerializableEvents.Moved:
                        yield return ReadMoved(reader);
                        break;
                    case SerializableEvents.CreatureCreated:
                        yield return ReadCreatureCreated(reader);
                        break;
                    case SerializableEvents.HeroCreated:
                        yield return ReadHeroCreated(reader);
                        break;
                    case SerializableEvents.HeroDestroyed:
                        yield return ReadHeroDestroyed(reader);
                        break;
                    case SerializableEvents.PlayerAvatarSpawned:
                        yield return ReadPlayerAvatarSpawned(reader);
                        break;
                    case SerializableEvents.ProjectileCreated:
                        yield return ReadProjectileCreated(reader);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static Action<IEventsReciever> ReadTerrainCreated(BinaryReader reader)
        {
            var mapId = reader.ReadString();
            return r => r.TerrainCreated(mapId);
        }

        private static Action<IEventsReciever> ReadRigidBodyCreated(BinaryReader reader)
        {
            var id = reader.ReadIdentityOf<RigidBody>();
            var position = reader.ReadVector();

            return r => r.RigidBodyCreated(id, position);
        }

        private static Action<IEventsReciever> ReadRigidBodyDestroyed(BinaryReader reader)
        {
            var id = reader.ReadIdentityOf<RigidBody>();
            return r => r.RigidBodyDestroyed(id);
        }

        private static Action<IEventsReciever> ReadMoved(BinaryReader reader)
        {
            var id = reader.ReadIdentityOf<RigidBody>();
            var newPosition = reader.ReadVector();

            return r => r.Moved(id, newPosition);
        }

        private static Action<IEventsReciever> ReadCreatureCreated(BinaryReader reader)
        {
            var id = reader.ReadIdentityOf<Creature>();
            var rigidBodyId = reader.ReadIdentityOf<RigidBody>();

            return r => r.CreatureCreated(id, rigidBodyId);
        }

        private static Action<IEventsReciever> ReadHeroCreated(BinaryReader reader)
        {
            var creatureId = reader.ReadIdentityOf<Creature>();

            return r => r.HeroCreated(creatureId);
        }

        private static Action<IEventsReciever> ReadHeroDestroyed(BinaryReader reader)
        {
            return r => r.HeroDestroyed();
        }

        private static Action<IEventsReciever> ReadPlayerAvatarSpawned(BinaryReader reader)
        {
            var creatureId = reader.ReadIdentityOf<Creature>();

            return r => r.PlayerAvatarSpawned(creatureId);
        }

        private static Action<IEventsReciever> ReadProjectileCreated(BinaryReader reader)
        {
            var rigidBodyId = reader.ReadIdentityOf<RigidBody>();

            return r => r.ProjectileCreated(rigidBodyId);
        }
    }
}