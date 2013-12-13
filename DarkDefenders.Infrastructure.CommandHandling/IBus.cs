namespace DarkDefenders.Infrastructure.CommandHandling
{
    public interface IBus
    {
        void Publish(ICommand command);
    }
}