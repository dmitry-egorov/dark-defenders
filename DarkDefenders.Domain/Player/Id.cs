using System;
using Infrastructure.DDDEventSourcing.Domain;

namespace DarkDefenders.Domain.Player
{
    public class Id : Identity
    {
        public Id(Guid value) : base(value)
        {
        }
    }
}