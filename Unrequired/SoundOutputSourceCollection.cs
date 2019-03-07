﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace MessengerClient.BusinessLogic.Audio.Classes
{
    class SoundOutputSourceCollection<IdType> : IEnumerable<SoundOutputSource>
    {
        private ConcurrentDictionary<IdType, SoundOutputSource> outputSources;

        public SoundOutputSourceCollection()
        {
            outputSources = new ConcurrentDictionary<IdType, SoundOutputSource>();
        }

        public bool TryGet(IdType id, out SoundOutputSource soundSource) => outputSources.TryGetValue(id, out soundSource);

        public bool AddSource(IdType id)
        {
            bool exists = outputSources.ContainsKey(id);
            SoundOutputSource newSource = new SoundOutputSource();

            if (exists)
                return false;
            else
            {
                outputSources[id] = newSource;
                newSource.Play();

                return true;
            }
        }

        public bool DeleteSource(IdType id)
        {
            bool result = outputSources.TryGetValue(id, out SoundOutputSource soundSource);

            if (!result)
                return false;

            soundSource.Stop();
            soundSource.Dispose();

            //outputSources.Remove(id);
            outputSources.TryRemove(id, out SoundOutputSource temp);

            return true;
        }

        public void Clear()
        {
            IdType[] ids = outputSources.Keys.ToArray();

            foreach (IdType id in ids)
                DeleteSource(id);
        }

        public IEnumerator<SoundOutputSource> GetEnumerator() => outputSources.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
