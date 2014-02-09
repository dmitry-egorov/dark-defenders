﻿using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.RigidBodies;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Interfaces
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
        void PlayerAvatarSpawned(IdentityOf<Creature> creatureId);
        void ProjectileCreated(IdentityOf<RigidBody> rigidBodyId);
    }

    public interface IReciever
    {
        void Created<TEntity>(IdentityOf<TEntity> id, params object[] data);
        void Destroyed<TEntity>(IdentityOf<TEntity> id);
        void Event<TEntity>(IdentityOf<TEntity> id, params object[] data);
    }
}