using DarkDefenders.Domain.Data.Entities.Clocks;
using DarkDefenders.Domain.Data.Entities.Creatures;
using DarkDefenders.Domain.Data.Entities.Heroes;
using DarkDefenders.Domain.Data.Entities.Projectiles;
using DarkDefenders.Domain.Data.Entities.RigidBodies;
using DarkDefenders.Domain.Data.Entities.Terrains;
using DarkDefenders.Domain.Data.Entities.Worlds;

namespace DarkDefenders.Domain.Data.Infrastructure
{
    public interface IEventDataReciever
    {
        void Recieve(ClockCreatedData clockCreatedData);
        void Recieve(TimeChangedData timeChangedData);
        void Recieve(WorldCreatedData worldCreatedData);
        void Recieve(HeroSpawnActivationTimeChangedData heroSpawnActivationTimeChangedData);
        void Recieve(SpawnHeroesChangedData spawnHeroesChangedData);
        void Recieve(ProjectileCreatedData projectileCreatedData);
        void Recieve(ProjectileDestroyedData projectileDestroyedData);
        void Recieve(RigidBodyCreatedData rigidBodyCreatedData);
        void Recieve(RigidBodyDestroyedData rigidBodyDestroyedData);
        void Recieve(AcceleratedData acceleratedData);
        void Recieve(MovedData movedData);
        void Recieve(AcceleratedAndMovedData acceleratedAndMovedData);
        void Recieve(ExternalForceChangedData externalForceChangedData);
        void Recieve(CreatureCreatedData creatureCreatedData);
        void Recieve(CreatureDestroyedData creatureDestroyedData);
        void Recieve(FiredData firedData);
        void Recieve(MovementChangedData movementChangedData);
        void Recieve(HeroCreatedData heroCreatedData);
        void Recieve(HeroDestroyedData heroDestroyedData);
        void Recieve(StateChangedData stateChangedData);
        void Recieve(TerrainCreatedData terrainCreatedData);
        void Recieve(PlayerAvatarSpawnedData playerAvatarSpawnedData);
    }
}