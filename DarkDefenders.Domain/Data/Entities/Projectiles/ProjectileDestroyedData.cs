using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.Projectiles;
using Infrastructure.Data;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Projectiles
{
    [ProtoContract]
    public class ProjectileDestroyedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<Projectile> ProjectileId { get; private set; }

        private ProjectileDestroyedData()//Protobuf
        {
        }

        public ProjectileDestroyedData(IdentityOf<Projectile> projectileId)
        {
            ProjectileId = projectileId;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}