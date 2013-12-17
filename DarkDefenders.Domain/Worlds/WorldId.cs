using System;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Worlds
{
    public class WorldId : Identity
    {
        public WorldId(Guid value) : base(value)
        {
        }

        public WorldId()
        {
        }
    }
}