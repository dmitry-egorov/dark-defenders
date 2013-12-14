namespace Infrastructure.DDDEventSourcing
{
    public interface ICommandPublisher
    {
        void Publish(ICommand command);
    }
}