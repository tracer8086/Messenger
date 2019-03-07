using System.Threading.Tasks;
using System.Threading;
using MessengerClient.BusinessLogic.Networking.Interfaces;
using MessengerClient.MessengerService;
using MessengerClient.Controllers.Interfaces;
using System.ServiceModel;
using System;
using System.Runtime.Remoting.Contexts;

namespace MessengerClient.BusinessLogic.Networking.Classes
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class ServiceConnector : IConnectionHandler, ISoundSender, ITextSender, IMessengerServiceCallback
    {
        private const int pulseInterval = 2000;

        private MessengerServiceClient client;
        private CancellationTokenSource tokenSource;
        private uint userId;

        public IUserController UserController { get; set; }
        public IUserSoundController SoundController { get; set; }
        public IUserTextMessageController TextMessageController { get; set; }
        public IDisconnectController DisconnectController { get; set; }
        public bool IsConnected { get; private set; }

        public ServiceConnector()
        {
            client = new MessengerServiceClient(new InstanceContext(this));
            tokenSource = new CancellationTokenSource();
            IsConnected = false;
        }

        private void HandleDisconnect()
        {
            IsConnected = false;
            client = null;
        }

        public bool Enter(string nickname, out string message)
        {
            if (client == null)
                client = new MessengerServiceClient(new InstanceContext(this));

            EnterData enterData;

            try
            {
                enterData = client.EnterService(nickname);
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

            userId = enterData.ID;

            string[] userList = client.GetUserList();

            UserController?.AddConnectedUsers(userList);

            try
            {
                client.NotifyClientIsReady(userId);
                IsConnected = true;
            }
            catch
            {
                client.Abort();
                HandleDisconnect();
                message = "Couldn't notify server that client is ready.";

                return false;
            }

            Task.Run(() =>
            {
                for (; ; )
                {
                    Task.Delay(pulseInterval).Wait();

                    try
                    {
                        if (tokenSource.IsCancellationRequested)
                            break;

                        client.Pulse(userId);
                    }
                    catch
                    {
                        client.Abort();
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
                tokenSource.Cancel();
                client.LeaveService(userId);
            }
            catch
            { }

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
                client.SendAudioMessage(new SoundToService() { UserID = userId, SoundBytes = sound });
            }
            catch
            { }
        }

        public void SendTextMessage(string message)
        {
            try
            {
                client.SendTextMessage(new MessageToService() { UserID = userId, Text = message });
            }
            catch
            { }
        }

        public void MakePulse() { }
    }
}