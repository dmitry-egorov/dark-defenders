using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.Projectiles;
using DarkDefenders.Domain.Entities.RigidBodies;
using Infrastructure.Data;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Projectiles
{
    [ProtoContract]
    public class ProjectileCreatedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<Projectile> ProjectileId { get; private set; }

        [ProtoMember(2)]
        public IdentityOf<RigidBody> RigidBodyId { get; private set; }

        private ProjectileCreatedData()//Protobuf
        {
        }

        public ProjectileCreatedData(IdentityOf<Projectile> projectileId, IdentityOf<RigidBody> rigidBodyId)
        {
            ProjectileId = projectileId;
            RigidBodyId = rigidBodyId;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}