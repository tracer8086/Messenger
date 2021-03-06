﻿using System.Runtime.Serialization;

namespace MessengerService.Contracts.DataContracts
{
    [DataContract]
    public class MessageToService
    {
        [DataMember]
        public uint UserId { get; set; }

        [DataMember]
        public string Text { get; set; }
    }
}