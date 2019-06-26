using System;
using MessengerClient.BusinessLogic.Audio.Interfaces;

namespace MessengerClient.BusinessLogic.Audio.Classes
{
    public class AudioOutput<IdType> : ISoundOutput<IdType>
    {
        private SoundOutputSourceCollection<IdType> outputSources;

        public AudioOutput()
        {
            outputSources = new SoundOutputSourceCollection<IdType>();
        }

        private bool AccessSources(IdType id, Action<SoundOutputSource> action)
        {
            var result = outputSources.TryGet(id, out SoundOutputSource player);

            if (!result)
                return false;

            action(player);

            return true;
        }

        private bool AccessSources(IdType id, Func<SoundOutputSource, bool> func)
        {
            var result = outputSources.TryGet(id, out SoundOutputSource player);

            if (!result || player == null)
                return false;

            return func(player);
        }

        public bool AddPlayer(IdType id) => outputSources.AddSource(id);

        public bool DeletePlayer(IdType id) => outputSources.DeleteSource(id);

        public bool PlaySound(IdType id, byte[] sound) => AccessSources(id, (player) => player.PlaySound(sound));

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