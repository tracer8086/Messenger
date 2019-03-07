using System;
using MessengerClient.BusinessLogic.TextVerification.Interfaces;

namespace MessengerClient.BusinessLogic.TextVerification.Classes.Verifiers
{
    public class MessageVerifier : IStringVerifier
    {
        public int PossibleLength { get; set; }

        public MessageVerifier(int possibleLength)
        {
            PossibleLength = possibleLength;
        }

        public bool VerifyString(string str)
        {
            if (String.IsNullOrWhiteSpace(str))
                return false;

            if (str.Length > PossibleLength)
                return false;

            return true;
        }
    }
}