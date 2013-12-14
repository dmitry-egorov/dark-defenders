using System;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Terrains
{
    public class TerrainId : Identity
    {
        public TerrainId(Guid value) : base(value)
        {
        }
    }
}