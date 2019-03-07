using System.Runtime.Serialization;

namespace MessengerService.Contracts.DataContracts
{
    [DataContract]
    public class MessageToUser
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Text { get; set; }
    }
}