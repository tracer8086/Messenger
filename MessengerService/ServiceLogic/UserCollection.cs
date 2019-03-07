using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections;
using MessengerService.Contracts.Callbacks;
using System.ServiceModel;

namespace MessengerService.ServiceLogic
{
    public class UserCollection<IdType> : IEnumerable<KeyValuePair<IdType, User>>
    {
        private ConcurrentDictionary<IdType, User> userDict;
        private object userCollectionLocker;

        public UserCollection()
        {
            userDict = new ConcurrentDictionary<IdType, User>();
            userCollectionLocker = new object();
        }

        public bool Contains(IdType id)
        {
            bool result;

            lock (userCollectionLocker)
                result =  userDict.ContainsKey(id);

            return result;
        }

        public User this[IdType id]
        {
            get
            {
                User user;

                lock (userCollectionLocker)
                    user = userDict[id];

                return user;
            }
        }

        public void AddUser(IdType id, string name, IClientCallback clientCallback, IClientChannel clientChannel)
        {
            if (userDict.ContainsKey(id))
                return;

            userDict.TryAdd(id, new User(name, clientCallback, clientChannel));
        }

        public void DeleteUser(IdType id)
        {
            if (!userDict.ContainsKey(id))
                return;

            userDict.TryRemove(id, out User temp);
        }

        public void Clear() => userDict.Clear();

        public string[] GetUserNames()
        {
            string[] userNames;

            lock (userCollectionLocker)
                userNames = (from elem in userDict
                             select elem.Value.Name).ToArray();

            return userNames;
        }

        public IEnumerator<KeyValuePair<IdType, User>> GetEnumerator() => userDict.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}