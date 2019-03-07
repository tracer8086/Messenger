using MessengerService.Contracts.Callbacks;
using MessengerService.Contracts.DataContracts;
using System.ServiceModel;

namespace MessengerService.ServiceLogic
{
    public class User
    {
        private IClientCallback clientCallback;
        private IClientChannel clientChannel;

        public string Name { get; }
        public bool IsReady { get; set; }

        public User(string name, IClientCallback clientCallback, IClientChannel clientChannel)
        {
            Name = name;
            this.clientCallback = clientCallback;
            IsReady = false;
            this.clientChannel = clientChannel;
        }

        public void ReceiveTextMessage(MessageToUser message) => clientCallback?.ReceiveTextMessage(message);

        public void ReceiveAudioMessage(SoundToUser soundMessage) => clientCallback?.ReceiveAudioMessage(soundMessage);

        public void NotifyUserConnected(string username) => clientCallback?.NotifyUserConnected(username);

        public void NotifyUserDisconnected(string username) => clientCallback?.NotifyUserDisconnected(username);

        public void Pulse() => clientCallback?.MakePulse();

        public void AbortConnection() => clientChannel?.Abort();
    }
}