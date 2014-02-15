using DarkDefenders.Domain.Model.Entities.Creatures;
using DarkDefenders.Domain.Model.Entities.RigidBodies;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Model
{
    public interface IEventsReciever
    {
        void TerrainCreated(string mapId);
        void RigidBodyCreated(IdentityOf<RigidBody> id, Vector position);
        void RigidBodyDestroyed(IdentityOf<RigidBody> id);
        void Moved(IdentityOf<RigidBody> id, Vector newPosition);
        void CreatureCreated(IdentityOf<Creature> id, IdentityOf<RigidBody> rigidBodyId);
        void HeroCreated(IdentityOf<Creature> creatureId);
        void HeroDestroyed();
        void PlayerCreated(IdentityOf<Creature> creatureId);
        void ProjectileCreated(IdentityOf<RigidBody> rigidBodyId);
    }

//    public interface IRemoteEvents
//    {
//        void MapLoaded(string mapId);
//        void Added(IdentityOf<ViewEntity> id, Vector initialPosition, ViewEntityType type);
//        void Moved(IdentityOf<ViewEntity> id, Vector newPosition)
//        void Removed(IdentityOf<ViewEntity> id);
//    }

//    LsitenFor<Terrain>(() => new TerrainListener())


//    public interface IReciever
//    {
//        void Created<TEntity>(IdentityOf<TEntity> id, params object[] data);
//        void Destroyed<TEntity>(IdentityOf<TEntity> id);
//        void Event<TEntity>(IdentityOf<TEntity> id, params object[] data);
//    }
}