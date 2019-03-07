using System.Runtime.Serialization;

namespace MessengerService.Contracts.DataContracts
{
    [DataContract]
    public class SoundToService
    {
        [DataMember]
        public uint UserID { get; set; }

        [DataMember]
        public byte[] SoundBytes { get; set; }
    }
}