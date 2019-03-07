namespace MessengerClient.BusinessLogic.Networking.Interfaces
{
    public interface IConnectionHandler : IConnectable
    {
        bool Enter(string nickname, out string message);
        void Leave();
    }
}