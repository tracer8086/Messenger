using System;
using System.Linq;
using System.ServiceModel;
using MessengerService.Contracts.Callbacks;
using MessengerService.Contracts.DataContracts;
using MessengerService.Contracts.ServiceContracts;
using System.Threading.Tasks;

namespace MessengerService.ServiceLogic
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MessengerService : IMessengerService
    {
        private const uint MaxUsers = 16;
        private const int PulseInterval = 2000;

        private readonly IdHandler _idHandler;
        private readonly UserCollection<uint> _users;
        private IClientCallback ClientCallbackChannel => OperationContext.Current.GetCallbackChannel<IClientCallback>();
        private IClientChannel ClientChannel => OperationContext.Current.Channel as IClientChannel;

        public MessengerService()
        {
            _idHandler = new IdHandler(maxCount: MaxUsers);
            _users = new UserCollection<uint>();

            Task.Run(() =>
            {
                for (; ; )
                {
                    Task.Delay(PulseInterval).Wait();
                    NotifyUsers((user) => user.Pulse());
                }
            });
        }

        private void NotifyUsers(Action<User> notifyUser)
        {
            Parallel.ForEach(_users, (elem) =>
            {
                try
                {
                    notifyUser(elem.Value);
                }
                catch
                {
                    if (_users.Contains(elem.Key))
                        _users[elem.Key].AbortConnection();

                    RemoveUser(elem.Key);
                }
            });
        }

        private void RemoveUser(uint id)
        {
            if (!_users.Contains(id))
                return;

            _users[id].IsReady = false;

            string username = _users[id].Name;

            _users.DeleteUser(id);
            _idHandler.ReleaseId(id);
            NotifyUsers((user) => user.NotifyUserDisconnected(username));
        }

        public EnterData EnterService(string username)
        {
            if (_users.GetUserNames().Contains(username) || String.IsNullOrWhiteSpace(username))
                return new EnterData() { Id = 0, Status = false, Message = $"Name \'{username}\' is already taken by another user." };

            if (!_idHandler.HasFreeId)
                return new EnterData() { Id = 0, Status = false, Message = "Server is full." };

            uint id = _idHandler.GetId();

            _users.AddUser(id, username, ClientCallbackChannel, ClientChannel);
            NotifyUsers((user) =>
            {
                if (user.Name != username)
                    user.NotifyUserConnected(username);
            });

            return new EnterData { Id = id, Status = true, Message = "Connection succeeded." };
        }

        public string[] GetUserList() => _users.GetUserNames();

        public void LeaveService(uint id) => RemoveUser(id);

        public void NotifyClientIsReady(uint id) => _users[id].IsReady = true;

        public void SendAudioMessage(SoundToService sound)
        {
            if (!_users.Contains(sound.UserId))
                return;

            string username = _users[sound.UserId].Name;

            NotifyUsers((user) =>
            {
                if (user.IsReady && user.Name != username)
                    user.ReceiveAudioMessage(new SoundToUser() { Username = username, SoundBytes = sound.SoundBytes });
            });
        }

        public void SendTextMessage(MessageToService message)
        {
            if (!_users.Contains(message.UserId))
                return;

            string username = _users[message.UserId].Name;

            NotifyUsers((user) =>
            {
                if (user.IsReady)
                    user.ReceiveTextMessage(new MessageToUser() { Username = username, Text = message.Text });
            });
        }

        public void Pulse(uint id)
        {
            if (!_users.Contains(id))
                return;
        }
    }
}