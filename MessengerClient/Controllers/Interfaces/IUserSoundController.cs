namespace MessengerClient.Controllers.Interfaces
{
    public interface IUserSoundController
    {
        void GetSoundFromUser(string username, byte[] sound);
    }
}