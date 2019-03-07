namespace MessengerClient.Controllers.Interfaces
{
    public interface IUserTextMessageController
    {
        void GetTextMessageFromUser(string username, string message);
    }
}