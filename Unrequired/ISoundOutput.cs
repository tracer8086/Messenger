namespace MessengerClient.BusinessLogic.Audio.Interfaces
{
    interface ISoundOutput<IdType>
    {
        bool AddPlayer(IdType id);
        bool DeletePlayer(IdType id);
        bool PlaySound(IdType id, byte[] sound);
        bool SetPlayerVolume(IdType id, float volume);
        bool GetPlayerVolume(IdType id, out float volume);
        bool Mute(IdType id);
        void EnableAllPlayers();
        void DisableAllPlayers();
        void DeleteAllPlayers();
    }
}
