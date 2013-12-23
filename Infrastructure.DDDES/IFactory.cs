namespace Infrastructure.DDDES
{
    public interface IFactory<out TRoot, in TCreationEvent>
    {
        TRoot Handle(TCreationEvent creationEvent);
    }
}