namespace MessengerClient.BusinessLogic.Audio.Interfaces
{
    public interface ISoundOutput<IdType>
    {
        bool AddPlayer(IdType id);
        bool DeletePlayer(IdType id);
        bool PlaySound(IdType id, byte[] sound);
        void EnableAllPlayers();
        void DisableAllPlayers();
        void DeleteAllPlayers();
    }
}