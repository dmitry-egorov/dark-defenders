using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain.Exceptions
{
    public class RootAlreadyExistsException : System.Exception
    {
        public RootAlreadyExistsException(string name, Identity creatureId): base("Root {0} with id {1} already exists".FormatWith(name, creatureId))
        {
        }
    }
}