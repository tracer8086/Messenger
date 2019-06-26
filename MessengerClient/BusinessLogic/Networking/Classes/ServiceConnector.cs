using System.Threading.Tasks;
using System.Threading;
using MessengerClient.BusinessLogic.Networking.Interfaces;
using MessengerClient.MessengerService;
using MessengerClient.Controllers.Interfaces;
using System.ServiceModel;

namespace MessengerClient.BusinessLogic.Networking.Classes
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class ServiceConnector : IConnectionHandler, ISoundSender, ITextSender, IMessengerServiceCallback
    {
        private const int PulseInterval = 2000;

        private MessengerServiceClient _client;
        private readonly CancellationTokenSource _tokenSource;
        private uint _userId;

        public IUserController UserController { get; set; }
        public IUserSoundController SoundController { get; set; }
        public IUserTextMessageController TextMessageController { get; set; }
        public IDisconnectController DisconnectController { get; set; }
        public bool IsConnected { get; private set; }

        public ServiceConnector()
        {
            _client = new MessengerServiceClient(new InstanceContext(this));
            _tokenSource = new CancellationTokenSource();
            IsConnected = false;
        }

        private void HandleDisconnect()
        {
            IsConnected = false;
            _client = null;
        }

        public bool Enter(string nickname, out string message)
        {
            if (_client == null)
                _client = new MessengerServiceClient(new InstanceContext(this));

            EnterData enterData;

            try
            {
                enterData = _client.EnterService(nickname);
            }
            catch
            {
                HandleDisconnect();
                message = "Couldn't enter because service is unavailable.";

                return false;
            }

            message = enterData.Message;

            if (!enterData.Status)
                return false;

            _userId = enterData.Id;

            var userList = _client.GetUserList();

            UserController?.AddConnectedUsers(userList);

            try
            {
                _client.NotifyClientIsReady(_userId);
                IsConnected = true;
            }
            catch
            {
                _client.Abort();
                HandleDisconnect();
                message = "Couldn't notify server that client is ready.";

                return false;
            }

            Task.Run(() =>
            {
                for (; ; )
                {
                    Task.Delay(PulseInterval).Wait();

                    try
                    {
                        if (_tokenSource.IsCancellationRequested)
                            break;

                        _client.Pulse(_userId);
                    }
                    catch
                    {
                        _client.Abort();
                        HandleDisconnect();
                        DisconnectController?.HandleDisconnect("Connection aborted.");
                        break;
                    }
                }
            });

            return true;
        }

        public void Leave()
        {
            try
            {
                _tokenSource.Cancel();
                _client.LeaveService(_userId);
            }
            catch
            {
                // ignored
            }

            HandleDisconnect();
        }

        public void NotifyUserConnected(string username) => UserController?.AddUser(username);

        public void NotifyUserDisconnected(string username) => UserController?.RemoveUser(username);

        public void ReceiveAudioMessage(SoundToUser soundMessage) => SoundController?.GetSoundFromUser(soundMessage.Username, soundMessage.SoundBytes);

        public void ReceiveTextMessage(MessageToUser message) => TextMessageController?.GetTextMessageFromUser(message.Username, message.Text);

        public void SendAudioMessage(byte[] sound)
        {
            try
            {
                _client.SendAudioMessage(new SoundToService { UserId = _userId, SoundBytes = sound });
            }
            catch
            {
                // ignored
            }
        }

        public void SendTextMessage(string message)
        {
            try
            {
                _client.SendTextMessage(new MessageToService { UserId = _userId, Text = message });
            }
            catch
            {
                // ignored
            }
        }

        public void MakePulse() { }
    }
}