using System.Runtime.Serialization;

namespace MessengerService.Contracts.DataContracts
{
    [DataContract]
    public class SoundToUser
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public byte[] SoundBytes { get; set; }
    }
}