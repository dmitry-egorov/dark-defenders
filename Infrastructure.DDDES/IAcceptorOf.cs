namespace Infrastructure.DDDES
{
    public interface IAcceptorOf<in TVisitor>
    {
        void Accept(TVisitor visitor);
    }
}