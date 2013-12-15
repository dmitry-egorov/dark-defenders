using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain.Exceptions
{
    public class RootDoesntExistException : System.Exception
    {
        public RootDoesntExistException(string name, Identity id)
            : base("Root {0} with id {1} doesn't exist".FormatWith(name, id))
        {
        }
    }
}