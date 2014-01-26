namespace DarkDefenders.Dtos.Infrastructure
{
    public interface IEventDto
    {
        void Accept(IEventDtoReciever reciever);
    }
}