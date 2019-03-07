namespace MessengerClient.GUI.Interfaces
{
    interface IVolumeSetter : IControl
    {
        string GetUserForSetVolume();
        float GetNewVolumeForSetVolume();
    }
}