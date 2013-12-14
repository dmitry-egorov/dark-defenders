namespace Infrastructure.DDDES.Implementations.Domain.Exceptions
{
    internal class RootDoesntExistException : System.Exception
    {
        public RootDoesntExistException() : base("Root doesn't exist")
        {
        }
    }
}