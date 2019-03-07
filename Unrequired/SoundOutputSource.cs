using System;
using NAudio.Wave;

namespace MessengerClient.BusinessLogic.Audio.Classes
{
    public class SoundOutputSource
    {
        // For sound output using output device.
        private WaveOut outputSound;
        // Buffer for output.
        private BufferedWaveProvider bufferStream;
        private bool isDisposed;
        private bool mute;
        private float savedVolume;
        private object lockObj;

        public bool IsPlaying { get; private set; }
        public bool IsMuted => mute;
        public float Volume
        {
            get
            {
                if (mute)
                    return savedVolume;

                return outputSound.Volume;
            }

            set
            {
                if (!mute)
                    outputSound.Volume = value;
                else
                    savedVolume = value;
            }
        }

        public SoundOutputSource()
        {
            outputSound = new WaveOut();
            bufferStream = new BufferedWaveProvider(new WaveFormat(8000, 16, 1));
            outputSound.Init(bufferStream);
            IsPlaying = false;
            isDisposed = false;
            mute = false;
            lockObj = new Object();
        }

        public bool PlaySound(byte[] sound)
        {
            if (!IsPlaying || isDisposed)
                return false;

            // If sound array is too big.
            try
            {
                if (outputSound != null)
                {
                    bufferStream.AddSamples(sound, 0, sound.GetLength(0));
                }
            }
            catch
            {
                bufferStream.ClearBuffer();
                return false;
            }

            return true;
        }

        public bool Play()
        {
            if (!IsPlaying)
            {
                outputSound.Play();
                IsPlaying = true;

                return true;
            }

            return false;
        }

        public bool Stop()
        {
            if (IsPlaying)
            {
                outputSound.Stop();
                IsPlaying = false;

                return true;
            }

            return false;
        }

        public void Mute()
        {
            if (!mute)
            {
                savedVolume = outputSound.Volume;
                outputSound.Volume = 0;
                mute = true;
            }
            else
            {
                outputSound.Volume = savedVolume;
                mute = false;
            }
        }

        private void CleanUp()
        {
            if (!isDisposed)
            {
                lock (lockObj)
                {
                    if (!isDisposed)
                    {
                        Stop();
                        outputSound?.Dispose();
                        isDisposed = true;
                    }
                }
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