using System;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Events
{
    public interface ICooldownEvents : IEntityEvents
    {
        void Created(TimeSpan cooldownDelay);
        void Activated(TimeSpan activationTime);
    }
}