using System;

namespace MessengerClient.BusinessLogic.Audio.Interfaces
{
    public interface ISoundInput
    {
        event Action<byte[]> OnSoundRecorded;
        bool IsEnabled { get; }
        void Enable();
        void Disable();
    }
}