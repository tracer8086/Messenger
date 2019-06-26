using System;
using System.Runtime.Remoting.Contexts;
using MessengerClient.BusinessLogic.Audio.Interfaces;
using NAudio.Wave;

namespace MessengerClient.BusinessLogic.Audio.Classes
{
    public class MicrophoneInput : ISoundInput
    {
        private readonly WaveIn inputSound;
        private bool _isEnabled;

        public event Action<byte[]> OnSoundRecorded;
        public bool IsEnabled => _isEnabled;

        public MicrophoneInput()
        {
            inputSound = new WaveIn();
            inputSound.WaveFormat = new WaveFormat(8000, 16, 1);
            inputSound.DataAvailable += (sender, soundEventArgs) => OnSoundRecorded?.Invoke(soundEventArgs.Buffer);
            _isEnabled = false;
        }

        public void Enable()
        {
            if (IsEnabled)
                return;

            inputSound.StartRecording();
            _isEnabled = true;
        }

        public void Disable()
        {
            if (!IsEnabled)
                return;

            inputSound.StopRecording();
            _isEnabled = false;
        }

        ~MicrophoneInput()
        {
            Disable();
            inputSound.Dispose();
        }
    }
}
