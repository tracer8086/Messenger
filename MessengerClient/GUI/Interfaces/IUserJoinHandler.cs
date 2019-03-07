using System;

namespace MessengerClient.GUI.Interfaces
{
    public interface IUserJoinHandler : IControl
    {
        void NotifyUserJoined(DateTime time, string username);
        void NotifyClientJoined(DateTime time, string username);
    }
}