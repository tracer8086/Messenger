using System;

namespace MessengerClient.GUI.Interfaces
{
    public interface IUserLeaveHandler : IControl
    {
        void NotifyUserLeft(DateTime time, string username);
    }
}