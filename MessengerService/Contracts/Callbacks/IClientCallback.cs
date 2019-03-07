using System.ServiceModel;
using MessengerService.Contracts.DataContracts;

namespace MessengerService.Contracts.Callbacks
{
    public interface IClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveTextMessage(MessageToUser message);

        [OperationContract(IsOneWay = true)]
        void ReceiveAudioMessage(SoundToUser soundMessage);

        [OperationContract(IsOneWay = true)]
        void NotifyUserConnected(string username);

        [OperationContract(IsOneWay = true)]
        void NotifyUserDisconnected(string username);

        [OperationContract(IsOneWay = true)]
        void MakePulse();
    }
}