namespace MessengerClient.Controllers.Interfaces
{
    public interface IUserController
    {
        void AddUser(string name);
        void RemoveUser(string name);
        void AddConnectedUsers(string[] names);
    }
}