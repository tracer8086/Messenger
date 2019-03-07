using System.Runtime.Serialization;

namespace MessengerService.Contracts.DataContracts
{
    [DataContract]
    public class EnterData
    {
        [DataMember]
        public uint ID { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}