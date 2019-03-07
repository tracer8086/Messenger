namespace MessengerClient.BusinessLogic.Networking.Interfaces
{
    public interface ITextSender : IConnectable
    {
        void SendTextMessage(string message);
    }
}