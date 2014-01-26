using Infrastructure.Math;
using Infrastructure.Physics;
using Infrastructure.Util;

namespace DarkDefenders.Dtos.Entities.RigidBodies
{
    public class RigidBodyInitialProperties: SlowValueObject<RigidBodyInitialProperties>
    {
        public Momentum InitialMomentum { get; private set; }
        public Vector Position { get; private set; }
        public RigidBodyProperties Properties { get; private set; }

        public RigidBodyInitialProperties(Momentum initialMomentum, Vector position, RigidBodyProperties properties)
        {
            InitialMomentum = initialMomentum;
            Position = position;
            Properties = properties;
        }
    }
}