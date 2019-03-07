using System;

namespace MessengerClient.GUI.Interfaces
{
    public interface IErrorMessageOutput : IControl
    {
        void OutputErrorMessage(DateTime time, string sender, string message);
    }
}