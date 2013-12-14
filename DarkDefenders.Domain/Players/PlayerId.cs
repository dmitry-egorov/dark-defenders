using System;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Players
{
    public class PlayerId : Identity
    {
        public PlayerId(Guid value) : base(value)
        {
        }
    }
}