namespace Infrastructure.DDDEventSourcing.Implementations
{
    public class Bus: ICommandPublisher
    {
        private readonly IEventStore _eventStore;
        private readonly ICommandProcessor<ICommand> _processor;

        public Bus(ICommandProcessor<ICommand> processor, IEventStore eventStore)
        {
            _eventStore = eventStore;
            _processor = processor;
        }

        public void Publish(ICommand command)
        {
            var events = _processor.Process(command);

            _eventStore.Append(command.RootId, events);
        }
    }
}