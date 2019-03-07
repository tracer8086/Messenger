using System;
using System.Windows.Forms;
using MessengerClient.Entry;
using MessengerClient.Controllers.Classes;
using MessengerClient.GUI.Classes;
using MessengerClient.BusinessLogic.Audio.Classes;
using MessengerClient.BusinessLogic.Networking.Classes;
using MessengerClient.BusinessLogic.TextVerification.Classes.Transformers;
using MessengerClient.BusinessLogic.TextVerification.Classes.Verifiers;

namespace MessengerClient
{
    static class Program
    {
        public const int PossibleLength = 256;

        /// <summary>
        /// The main application entry point.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Controller.
            Controller controller = new Controller();

            // GUI.
            EnterForm enterForm = new EnterForm();
            ChatForm chatWindow = new ChatForm();
            ErrorForm errorForm = new ErrorForm();

            controller.EnterForm = enterForm;
            controller.UserList = chatWindow;
            controller.TextMessageInput = chatWindow;
            controller.TextMessageOutput = chatWindow;
            controller.UserJoinHandler = chatWindow;
            controller.UserLeaveHandler = chatWindow;
            controller.ErrorMessageOutput = errorForm;

            enterForm.ConnectionController = controller;
            chatWindow.TextMessageController = controller;
            chatWindow.ConnectionController = controller;
            chatWindow.SoundInputController = controller;
            chatWindow.SoundOutputController = controller;

            // Sound.
            AudioOutput<string> audioOutput = new AudioOutput<string>();
            MicrophoneInput microphoneInput = new MicrophoneInput();

            controller.SoundOutput = audioOutput;
            controller.SoundInput = microphoneInput;

            // Networking.
            ServiceConnector serviceConnector = new ServiceConnector();

            controller.TextSender = serviceConnector;
            controller.SoundSender = serviceConnector;
            controller.ConnectionHandler = serviceConnector;

            serviceConnector.UserController = controller;
            serviceConnector.SoundController = controller;
            serviceConnector.TextMessageController = controller;
            serviceConnector.DisconnectController = controller;

            // Text processing.
            MessageTransformer messageTransformer = new MessageTransformer();
            NicknameTransformer nicknameTransformer = new NicknameTransformer();
            MessageVerifier messageVerifier = new MessageVerifier(possibleLength: PossibleLength);
            NicknameVerifier nicknameVerifier = new NicknameVerifier();

            controller.MessageTransformer = messageTransformer;
            controller.NicknameTransformer = nicknameTransformer;
            controller.MessageVerifier = messageVerifier;
            controller.NicknameVerifier = nicknameVerifier;

            Application.Run(new MessengerAppContext(enterForm, chatWindow, errorForm));
        }
    }
}