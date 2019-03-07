namespace MessengerClient.GUI.Interfaces
{
    public interface IUserList : IControl
    {
        void AddUser(string user);
        void RemoveUser(string user);
        void Clear();
    }
}