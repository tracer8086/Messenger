using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace MessengerClient.BusinessLogic.Audio.Classes
{
    public class SoundOutputSourceCollection<IdType> : IEnumerable<SoundOutputSource>
    {
        private readonly ConcurrentDictionary<IdType, SoundOutputSource> _outputSources;

        public SoundOutputSourceCollection()
        {
            _outputSources = new ConcurrentDictionary<IdType, SoundOutputSource>();
        }

        public bool TryGet(IdType id, out SoundOutputSource soundSource) => _outputSources.TryGetValue(id, out soundSource);

        public bool AddSource(IdType id)
        {
            var exists = _outputSources.ContainsKey(id);
            var newSource = new SoundOutputSource();

            if (exists)
                return false;

            newSource.Play();

            return _outputSources.TryAdd(id, newSource);
        }

        public bool DeleteSource(IdType id)
        {
            var result = _outputSources.TryGetValue(id, out var soundSource);

            if (!result)
                return false;

            soundSource.Stop();
            soundSource.Dispose();

            return _outputSources.TryRemove(id, out _);
        }

        public void Clear()
        {
            var ids = _outputSources.Keys.ToArray();

            foreach (var id in ids)
                DeleteSource(id);
        }

        public IEnumerator<SoundOutputSource> GetEnumerator() => _outputSources.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}