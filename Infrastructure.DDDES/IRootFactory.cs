namespace Infrastructure.DDDES
{
    public interface IRootFactory<out TRoot, in TCreationEvent>
    {
        TRoot Handle(TCreationEvent creationEvent);
    }
}