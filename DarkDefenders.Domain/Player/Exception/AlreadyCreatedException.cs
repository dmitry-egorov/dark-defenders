using Infrastructure.Util;

namespace DarkDefenders.Domain.Player.Exception
{
    public class AlreadyCreatedException : System.Exception
    {
        public AlreadyCreatedException(Id id) : base("Player with id {0} already exists".FormatWith(id))
        {
        }
    }
}