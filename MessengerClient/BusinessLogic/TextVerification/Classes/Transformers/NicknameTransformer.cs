using MessengerClient.BusinessLogic.TextVerification.Interfaces;

namespace MessengerClient.BusinessLogic.TextVerification.Classes.Transformers
{
    public class NicknameTransformer : IStringTransformer
    {
        public string TransformString(string str) => str.Trim();
    }
}