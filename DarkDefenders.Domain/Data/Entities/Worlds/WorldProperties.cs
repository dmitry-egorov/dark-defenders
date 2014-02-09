using System;
using System.Collections.Generic;
using System.Linq;
using DarkDefenders.Domain.Data.Entities.Creatures;
using Infrastructure.Data;
using Infrastructure.Util;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Worlds
{
    [ProtoContract]
    public class WorldProperties: SlowValueObject
    {
        [ProtoMember(1)]
        public CreatureProperties PlayersAvatarProperties { get; private set; }
        [ProtoMember(2)]
        public List<VectorData> HeroesSpawnPositions { get; private set; }
        [ProtoMember(3)]
        public TimeSpan HeroesSpawnCooldown { get; private set; }
        [ProtoMember(4)]
        public CreatureProperties HeroesCreatureProperties { get; private set; }
        [ProtoMember(5)]
        public List<VectorData> PlayersSpawnPositions { get; private set; }

        public WorldProperties()//Protobuf
        {
        }

        public WorldProperties(IEnumerable<VectorData> playersSpawnPositions, CreatureProperties playersAvatarProperties, IEnumerable<VectorData> heroesSpawnPositions, TimeSpan heroesSpawnCooldown, CreatureProperties heroesCreatureProperties)
        {
            PlayersAvatarProperties = playersAvatarProperties;
            HeroesSpawnPositions = heroesSpawnPositions.ToList();
            HeroesSpawnCooldown = heroesSpawnCooldown;
            HeroesCreatureProperties = heroesCreatureProperties;
            PlayersSpawnPositions = playersSpawnPositions.ToList();
        }
    }
}