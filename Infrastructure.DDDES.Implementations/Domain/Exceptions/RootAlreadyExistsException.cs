using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain.Exceptions
{
    public class RootAlreadyExistsException : System.Exception
    {
        public RootAlreadyExistsException(Identity playerId) : base("Root with id {0} already exists".FormatWith(playerId))
        {
        }
    }
}