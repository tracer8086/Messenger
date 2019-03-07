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
        private const uint maxUsers = 16;
        private const int pulseInterval = 2000;

        private IdHandler idHandler;
        private UserCollection<uint> users;
        private IClientCallback clientCallbackChannel => OperationContext.Current.GetCallbackChannel<IClientCallback>();
        private IClientChannel clientChannel => OperationContext.Current.Channel as IClientChannel;

        public MessengerService()
        {
            idHandler = new IdHandler(maxCount: maxUsers);
            users = new UserCollection<uint>();

            Task.Run(() =>
            {
                for (; ; )
                {
                    Task.Delay(pulseInterval).Wait();
                    NotifyUsers((user) => user.Pulse());
                }
            });
        }

        private void NotifyUsers(Action<User> notifyUser)
        {
            Parallel.ForEach(users, (elem) =>
            {
                try
                {
                    notifyUser(elem.Value);
                }
                catch
                {
                    if (users.Contains(elem.Key))
                        users[elem.Key].AbortConnection();

                    RemoveUser(elem.Key);
                }
            });
        }

        private void RemoveUser(uint id)
        {
            if (!users.Contains(id))
                return;

            users[id].IsReady = false;

            string username = users[id].Name;

            users.DeleteUser(id);
            idHandler.ReleaseID(id);
            NotifyUsers((user) => user.NotifyUserDisconnected(username));
        }

        public EnterData EnterService(string username)
        {
            if (users.GetUserNames().Contains(username) || String.IsNullOrWhiteSpace(username))
                return new EnterData() { ID = 0, Status = false, Message = $"Name \'{username}\' is already taken by another user." };

            if (!idHandler.HasFreeID)
                return new EnterData() { ID = 0, Status = false, Message = "Server is full." };

            uint id = idHandler.GetId();

            users.AddUser(id, username, clientCallbackChannel, clientChannel);
            NotifyUsers((user) =>
            {
                if (user.Name != username)
                    user.NotifyUserConnected(username);
            });

            return new EnterData { ID = id, Status = true, Message = "Connection succeeded." };
        }

        public string[] GetUserList() => users.GetUserNames();

        public void LeaveService(uint id) => RemoveUser(id);

        public void NotifyClientIsReady(uint id) => users[id].IsReady = true;

        public void SendAudioMessage(SoundToService sound)
        {
            if (!users.Contains(sound.UserID))
                return;

            string username = users[sound.UserID].Name;

            NotifyUsers((user) =>
            {
                if (user.IsReady && user.Name != username)
                    user.ReceiveAudioMessage(new SoundToUser() { Username = username, SoundBytes = sound.SoundBytes });
            });
        }

        public void SendTextMessage(MessageToService message)
        {
            if (!users.Contains(message.UserID))
                return;

            string username = users[message.UserID].Name;

            NotifyUsers((user) =>
            {
                if (user.IsReady)
                    user.ReceiveTextMessage(new MessageToUser() { Username = username, Text = message.Text });
            });
        }

        public void Pulse(uint id)
        {
            if (!users.Contains(id))
                return;
        }
    }
}