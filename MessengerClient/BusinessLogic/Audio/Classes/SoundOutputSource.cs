using System;
using NAudio.Wave;

namespace MessengerClient.BusinessLogic.Audio.Classes
{
    public class SoundOutputSource
    {
        // For sound output using output device.
        private readonly WaveOut _outputSound;
        // Buffer for output.
        private readonly BufferedWaveProvider _bufferStream;
        private bool _isDisposed;
        private bool _mute;
        private float _savedVolume;
        private readonly object _lockObj;

        public bool IsPlaying { get; private set; }
        public bool IsMuted => _mute;
        public float Volume
        {
            get => _mute ? _savedVolume : _outputSound.Volume;

            set
            {
                if (!_mute)
                    _outputSound.Volume = value;
                else
                    _savedVolume = value;
            }
        }

        public SoundOutputSource()
        {
            _outputSound = new WaveOut();
            _bufferStream = new BufferedWaveProvider(new WaveFormat(8000, 16, 1));
            _outputSound.Init(_bufferStream);
            IsPlaying = false;
            _isDisposed = false;
            _mute = false;
            _lockObj = new object();
        }

        public bool PlaySound(byte[] sound)
        {
            if (!IsPlaying || _isDisposed)
                return false;

            // If sound array is too big.
            try
            {
                if (_outputSound != null)
                {
                    _bufferStream.AddSamples(sound, 0, sound.GetLength(0));
                }
            }
            catch
            {
                _bufferStream.ClearBuffer();
                return false;
            }

            return true;
        }

        public bool Play()
        {
            if (IsPlaying)
                return false;

            _outputSound.Play();
            IsPlaying = true;

            return true;

        }

        public bool Stop()
        {
            if (!IsPlaying)
                return false;

            _outputSound.Stop();
            IsPlaying = false;

            return true;

        }

        public void Mute()
        {
            if (!_mute)
            {
                _savedVolume = _outputSound.Volume;
                _outputSound.Volume = 0;
                _mute = true;
            }
            else
            {
                _outputSound.Volume = _savedVolume;
                _mute = false;
            }
        }

        private void CleanUp()
        {
            if (_isDisposed)
                return;

            lock (_lockObj)
            {
                if (_isDisposed)
                    return;

                Stop();
                _outputSound?.Dispose();
                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            CleanUp();
            GC.SuppressFinalize(this);
        }

        ~SoundOutputSource() => CleanUp();
    }
}