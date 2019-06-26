using MessengerService.Contracts.Callbacks;
using MessengerService.Contracts.DataContracts;
using System.ServiceModel;

namespace MessengerService.ServiceLogic
{
    public class User
    {
        private readonly IClientCallback _clientCallback;
        private readonly IClientChannel _clientChannel;

        public string Name { get; }
        public bool IsReady { get; set; }

        public User(string name, IClientCallback clientCallback, IClientChannel clientChannel)
        {
            Name = name;
            this._clientCallback = clientCallback;
            IsReady = false;
            this._clientChannel = clientChannel;
        }

        public void ReceiveTextMessage(MessageToUser message) => _clientCallback?.ReceiveTextMessage(message);

        public void ReceiveAudioMessage(SoundToUser soundMessage) => _clientCallback?.ReceiveAudioMessage(soundMessage);

        public void NotifyUserConnected(string username) => _clientCallback?.NotifyUserConnected(username);

        public void NotifyUserDisconnected(string username) => _clientCallback?.NotifyUserDisconnected(username);

        public void Pulse() => _clientCallback?.MakePulse();

        public void AbortConnection() => _clientChannel?.Abort();
    }
}