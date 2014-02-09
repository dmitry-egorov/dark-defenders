using DarkDefenders.Domain.Data.Entities.Clocks;
using DarkDefenders.Domain.Data.Entities.Creatures;
using DarkDefenders.Domain.Data.Entities.Heroes;
using DarkDefenders.Domain.Data.Entities.Projectiles;
using DarkDefenders.Domain.Data.Entities.RigidBodies;
using DarkDefenders.Domain.Data.Entities.Terrains;
using DarkDefenders.Domain.Data.Entities.Worlds;
using Infrastructure.Util;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Infrastructure
{
    [ProtoContract]
    [ProtoInclude(1, typeof(ClockCreatedData))]
    [ProtoInclude(2, typeof(TerrainCreatedData))]
    [ProtoInclude(3, typeof(TimeChangedData))]
    [ProtoInclude(4, typeof(WorldCreatedData))]
    [ProtoInclude(5, typeof(HeroSpawnActivationTimeChangedData))]
    [ProtoInclude(6, typeof(SpawnHeroesChangedData))]
    [ProtoInclude(7, typeof(ProjectileCreatedData))]
    [ProtoInclude(8, typeof(ProjectileDestroyedData))]
    [ProtoInclude(9, typeof(RigidBodyCreatedData))]
    [ProtoInclude(10, typeof(RigidBodyDestroyedData))]
    [ProtoInclude(11, typeof(AcceleratedData))]
    [ProtoInclude(12, typeof(MovedData))]
    [ProtoInclude(13, typeof(AcceleratedAndMovedData))]
    [ProtoInclude(14, typeof(ExternalForceChangedData))]
    [ProtoInclude(15, typeof(CreatureCreatedData))]
    [ProtoInclude(16, typeof(CreatureDestroyedData))]
    [ProtoInclude(17, typeof(FiredData))]
    [ProtoInclude(18, typeof(MovementChangedData))]
    [ProtoInclude(19, typeof(HeroCreatedData))]
    [ProtoInclude(20, typeof(HeroDestroyedData))]
    [ProtoInclude(21, typeof(StateChangedData))]
    [ProtoInclude(22, typeof(PlayerAvatarSpawnedData))]
    public abstract class EventDataBase : SlowValueObject
    {
        public abstract void Accept(IEventDataReciever reciever);
    }
}