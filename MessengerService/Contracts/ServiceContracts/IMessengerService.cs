using System.ServiceModel;
using MessengerService.Contracts.Callbacks;
using MessengerService.Contracts.DataContracts;

namespace MessengerService.Contracts.ServiceContracts
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IClientCallback))]
    public interface IMessengerService
    {
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        EnterData EnterService(string username);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = true)]
        void LeaveService(uint id);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void SendTextMessage(MessageToService message);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void SendAudioMessage(SoundToService sound);

        [OperationContract(IsOneWay = false, IsInitiating = false, IsTerminating = false)]
        string[] GetUserList();

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void NotifyClientIsReady(uint id);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void Pulse(uint id);
    }
}