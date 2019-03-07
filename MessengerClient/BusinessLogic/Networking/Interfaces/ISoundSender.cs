namespace MessengerClient.BusinessLogic.Networking.Interfaces
{
    public interface ISoundSender : IConnectable
    {
        void SendAudioMessage(byte[] sound);
    }
}