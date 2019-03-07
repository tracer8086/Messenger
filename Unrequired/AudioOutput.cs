using System;
using MessengerClient.BusinessLogic.Audio.Interfaces;

namespace MessengerClient.BusinessLogic.Audio.Classes
{
    class AudioOutput<IdType> : ISoundOutput<IdType>
    {
        private SoundOutputSourceCollection<IdType> outputSources;

        public AudioOutput()
        {
            outputSources = new SoundOutputSourceCollection<IdType>();
        }

        private bool AccessSources(IdType id, Action<SoundOutputSource> action)
        {
            bool result = outputSources.TryGet(id, out SoundOutputSource player);

            if (!result)
                return false;

            action(player);

            return true;
        }

        private bool AccessSources(IdType id, Func<SoundOutputSource, bool> func)
        {
            bool result = outputSources.TryGet(id, out SoundOutputSource player);

            if (!result || player == null)
                return false;

            return func(player);
        }

        public bool AddPlayer(IdType id) => outputSources.AddSource(id);

        public bool DeletePlayer(IdType id) => outputSources.DeleteSource(id);

        public bool PlaySound(IdType id, byte[] sound) => AccessSources(id, (player) => player.PlaySound(sound));

        public bool GetPlayerVolume(IdType id, out float volume)
        {
            volume = -1;

            bool result = outputSources.TryGet(id, out SoundOutputSource player);

            if (!result)
                return false;

            volume = player.Volume;

            return true;
        }

        public bool SetPlayerVolume(IdType id, float volume) => AccessSources(id, (player) => player.Volume = volume);

        public bool Mute(IdType id) => AccessSources(id, (player) => player.Mute());

        public void EnableAllPlayers()
        {
            foreach (SoundOutputSource player in outputSources)
                if (player.IsMuted)
                    player.Mute();
        }

        public void DisableAllPlayers()
        {
            foreach (SoundOutputSource player in outputSources)
                if (!player.IsMuted)
                    player.Mute();
        }

        public void DeleteAllPlayers() => outputSources.Clear();
    }
}