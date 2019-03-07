using System;

namespace MessengerClient.GUI.Interfaces
{
    public interface ITextMessageOutput : IControl
    {
        void OutputMessage(DateTime time, string sender, string message);
        void ClearChat();
    }
}