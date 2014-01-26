using DarkDefenders.Dtos.Entities.Clocks;
using DarkDefenders.Dtos.Entities.Creatures;
using DarkDefenders.Dtos.Entities.Heroes;
using DarkDefenders.Dtos.Entities.Projectiles;
using DarkDefenders.Dtos.Entities.RigidBodies;
using DarkDefenders.Dtos.Entities.Terrains;
using DarkDefenders.Dtos.Entities.Worlds;

namespace DarkDefenders.Dtos.Infrastructure
{
    public interface IEventDtoReciever
    {
        void Recieve(ClockCreatedDto clockCreatedDto);
        void Recieve(TimeChangedDto timeChangedDto);
        void Recieve(WorldCreatedDto worldCreatedDto);
        void Recieve(HeroSpawnActivationTimeChangedDto heroSpawnActivationTimeChangedDto);
        void Recieve(SpawnHeroesChangedDto spawnHeroesChangedDto);
        void Recieve(ProjectileCreatedDto projectileCreatedDto);
        void Recieve(ProjectileDestroyedDto projectileDestroyedDto);
        void Recieve(RigidBodyCreatedDto rigidBodyCreatedDto);
        void Recieve(RigidBodyDestroyedDto rigidBodyDestroyedDto);
        void Recieve(AcceleratedDto acceleratedDto);
        void Recieve(MovedDto movedDto);
        void Recieve(AcceleratedAndMovedDto acceleratedAndMovedDto);
        void Recieve(ExternalForceChangedDto externalForceChangedDto);
        void Recieve(CreatureCreatedDto creatureCreatedDto);
        void Recieve(CreatureDestroyedDto creatureDestroyedDto);
        void Recieve(FiredDto firedDto);
        void Recieve(MovementChangedDto movementChangedDto);
        void Recieve(HeroCreatedDto heroCreatedDto);
        void Recieve(HeroDestroyedDto heroDestroyedDto);
        void Recieve(StateChangedDto stateChangedDto);
        void Recieve(TerrainCreatedDto terrainCreatedDto);
        void Recieve(PlayerAvatarSpawnedDto playerAvatarSpawnedDto);
    }
}