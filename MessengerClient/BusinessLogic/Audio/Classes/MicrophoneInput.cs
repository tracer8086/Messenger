using System;
using System.Runtime.Remoting.Contexts;
using MessengerClient.BusinessLogic.Audio.Interfaces;
using NAudio.Wave;

namespace MessengerClient.BusinessLogic.Audio.Classes
{
    public class MicrophoneInput : ISoundInput
    {
        private WaveIn inputSound;
        private bool isEnabled;

        public event Action<byte[]> OnSoundRecorded;
        public bool IsEnabled => isEnabled;

        public MicrophoneInput()
        {
            inputSound = new WaveIn();
            inputSound.WaveFormat = new WaveFormat(8000, 16, 1);
            inputSound.DataAvailable += (sender, soundEventArgs) => OnSoundRecorded?.Invoke(soundEventArgs.Buffer);
            isEnabled = false;
        }

        public void Enable()
        {
            if (!IsEnabled)
            {
                inputSound.StartRecording();
                isEnabled = true;
            }
        }

        public void Disable()
        {
            if (IsEnabled)
            {
                inputSound.StopRecording();
                isEnabled = false;
            }
        }

        ~MicrophoneInput()
        {
            Disable();
            inputSound.Dispose();
        }
    }
}
