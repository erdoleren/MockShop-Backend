namespace MockShop.Application.Interfaces
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
    }
}
