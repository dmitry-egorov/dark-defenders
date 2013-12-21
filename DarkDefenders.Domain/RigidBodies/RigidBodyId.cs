using System;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.RigidBodies
{
    public class RigidBodyId : Identity
    {
        public RigidBodyId(Guid value) : base(value)
        {
        }

        public RigidBodyId()
        {
        }
    }
}