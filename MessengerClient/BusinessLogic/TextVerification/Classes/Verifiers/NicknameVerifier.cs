using MessengerClient.BusinessLogic.TextVerification.Interfaces;
using System.Text.RegularExpressions;

namespace MessengerClient.BusinessLogic.TextVerification.Classes.Verifiers
{
    public class NicknameVerifier : IStringVerifier
    {
        private Regex nicknameVerifierRegex;

        public NicknameVerifier()
        {
            nicknameVerifierRegex = new Regex(@"^\w{4,16}$", RegexOptions.Compiled);
        }

        public bool VerifyString(string str)
        {
            if (str == null)
                return false;

            bool isMatch = nicknameVerifierRegex.IsMatch(str);

            return isMatch;
        }
    }
}