using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections;
using MessengerService.Contracts.Callbacks;
using System.ServiceModel;

namespace MessengerService.ServiceLogic
{
    public class UserCollection<TIdType> : IEnumerable<KeyValuePair<TIdType, User>>
    {
        private readonly ConcurrentDictionary<TIdType, User> _userDict;
        private readonly object _userCollectionLocker;

        public UserCollection()
        {
            _userDict = new ConcurrentDictionary<TIdType, User>();
            _userCollectionLocker = new object();
        }

        public bool Contains(TIdType id)
        {
            bool result;

            lock (_userCollectionLocker)
                result =  _userDict.ContainsKey(id);

            return result;
        }

        public User this[TIdType id]
        {
            get
            {
                User user;

                lock (_userCollectionLocker)
                    user = _userDict[id];

                return user;
            }
        }

        public void AddUser(TIdType id, string name, IClientCallback clientCallback, IClientChannel clientChannel)
        {
            if (_userDict.ContainsKey(id))
                return;

            _userDict.TryAdd(id, new User(name, clientCallback, clientChannel));
        }

        public void DeleteUser(TIdType id)
        {
            if (!_userDict.ContainsKey(id))
                return;

            _userDict.TryRemove(id, out _);
        }

        public void Clear() => _userDict.Clear();

        public string[] GetUserNames()
        {
            string[] userNames;

            lock (_userCollectionLocker)
                userNames = (from elem in _userDict
                             select elem.Value.Name).ToArray();

            return userNames;
        }

        public IEnumerator<KeyValuePair<TIdType, User>> GetEnumerator() => _userDict.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}