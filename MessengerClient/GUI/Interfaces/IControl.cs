namespace MessengerClient.GUI.Interfaces
{
    public interface IControl
    {
        bool IsVisible { get; }
        void ShowControl();
        void HideControl();
    }
}