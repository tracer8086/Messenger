using System;
using MessengerClient.Controllers.Interfaces;
using MessengerClient.BusinessLogic.Audio.Interfaces;
using MessengerClient.BusinessLogic.Networking.Interfaces;
using MessengerClient.BusinessLogic.TextVerification.Interfaces;
using MessengerClient.GUI.Interfaces;

namespace MessengerClient.Controllers.Classes
{
    class Controller :
        IConnectionController,
        ISoundOutputController,
        ISoundInputController,
        ITextMessageController,
        IUserController,
        IUserSoundController,
        IUserTextMessageController,
        IDisconnectController
    {
        // GUI.
        public ITextMessageOutput TextMessageOutput { get; set; }
        public IUserList UserList { get; set; }
        public IErrorMessageOutput ErrorMessageOutput { get; set; }
        public IEnterForm EnterForm { get; set; }
        public IMute MuteForm { get; set; }
        public IVolumeGetter VolumeGetter { get; set; }
        public IVolumeOutputter VolumeOutputter { get; set; }
        public IVolumeSetter VolumeSetter { get; set; }
        public ITextMessageInput TextMessageInput { get; set; }
        public IUserJoinHandler UserJoinHandler { get; set; }
        public IUserLeaveHandler UserLeaveHandler { get; set; }

        // Sound work.
        private ISoundInput soundInput;

        public ISoundInput SoundInput
        {
            get => soundInput;

            set
            {
                soundInput = value;
                soundInput.OnSoundRecorded += SendAudioMessage;
            }
        }
        public ISoundOutput<string> SoundOutput { get; set; }

        // Networking.
        public IConnectionHandler ConnectionHandler { get; set; }
        public ISoundSender SoundSender { get; set; }
        public ITextSender TextSender { get; set; }

        // Text processing.
        public IStringTransformer MessageTransformer { get; set; }
        public IStringTransformer NicknameTransformer { get; set; }
        public IStringVerifier MessageVerifier { get; set; }
        public IStringVerifier NicknameVerifier { get; set; }

        private void SendAudioMessage(byte[] sound)
        {
            if (SoundSender != null && SoundSender.IsConnected)
                SoundSender.SendAudioMessage(sound);
        }

        private void ShowControl(IControl control)
        {
            if (control != null && !control.IsVisible)
                control.ShowControl();
        }

        private void HideControl(IControl control)
        {
            if (control != null && control.IsVisible)
                control.HideControl();
        }

        private void OutputError(DateTime dateTime, string sender, string message)
        {
            ShowControl(ErrorMessageOutput);
            ErrorMessageOutput.OutputErrorMessage(dateTime, sender, message);
        }

        private bool VerifyNickname(string nickname)
        {
            if (NicknameVerifier == null)
            {
                OutputError(DateTime.Now, "Nickname verifier", "Couldn't verify nickname because nickname verifier is inaccessible.");
                return false;
            }

            bool verificationResult = NicknameVerifier.VerifyString(nickname);

            if (!verificationResult)
            {
                OutputError(DateTime.Now, "Connection controller", $"Incorrect nickname: {nickname}; nickname must consist of 4 to 16 digits or letters.");
                return false;
            }

            return true;
        }

        private bool TransformNickname(ref string nickname)
        {
            if (NicknameTransformer == null)
            {
                OutputError(DateTime.Now, "Nickname processor", "Couldn't process nickname because nickname transformer is inaccessible.");
                return false;
            }

            nickname = NicknameTransformer.TransformString(nickname);

            return true;
        }

        private bool VerifyMessage(string message)
        {
            if (MessageVerifier == null)
            {
                OutputError(DateTime.Now, "Message verifier", "Couldn't verify message because message verifier is inaccessible.");
                return false;
            }

            bool verificationResult = MessageVerifier.VerifyString(message);

            if (!verificationResult)
            {
                OutputError(DateTime.Now, "Connection controller", $"Incorrect message: {message}; message is too long.");
                return false;
            }

            return true;
        }

        private bool TransformMessage(ref string message)
        {
            if (MessageTransformer == null)
            {
                OutputError(DateTime.Now, "Message processor", "Couldn't process message because message processor is inaccessible.");
                return false;
            }

            message = MessageTransformer.TransformString(message);

            return true;
        }

        private void ReturnToStartState()
        {
            HideControl(UserList);
            HideControl(TextMessageInput);
            HideControl(TextMessageOutput);
            ShowControl(EnterForm);

            SoundOutput?.DeleteAllPlayers();
            UserList?.Clear();
            TextMessageOutput?.ClearChat();
        }

        public void AddUser(string name)
        {
            if (SoundOutput != null)
            {
                bool result = SoundOutput.AddPlayer(name);

                if (!result)
                    OutputError(DateTime.Now, "Audio output", $"Couldn't add audioplayer for user {name}");
            }

            UserList?.AddUser(name);
            UserJoinHandler?.NotifyUserJoined(DateTime.Now, name);
        }

        public void DisableInput()
        {
            if (SoundInput != null && SoundInput.IsEnabled)
                SoundInput.Disable();
        }

        public void DisableOutput() => SoundOutput?.DisableAllPlayers();

        public void EnableInput()
        {
            if (SoundInput != null && !SoundInput.IsEnabled)
                SoundInput.Enable();
        }

        public void EnableOutput() => SoundOutput?.EnableAllPlayers();

        public void Enter()
        {
            if (ConnectionHandler != null && !ConnectionHandler.IsConnected && EnterForm != null)
            {
                string nickname = EnterForm.GetUserDataForEnter();
                bool verificationResult = VerifyNickname(nickname);

                if (!verificationResult)
                    return;

                bool transformationResult = TransformNickname(ref nickname);

                if (!transformationResult)
                    return;

                bool result = ConnectionHandler.Enter(nickname, out string connectorMessage);

                if (!result)
                {
                    OutputError(DateTime.Now, "Service connector", connectorMessage);
                    return;
                }

                HideControl(EnterForm);
                ShowControl(TextMessageOutput);
                ShowControl(UserList);
                ShowControl(TextMessageInput);

                if (SoundInput != null && !SoundInput.IsEnabled)
                    SoundInput.Enable();
            }
        }

        public void GetSoundFromUser(string username, byte[] sound)
        {
            if (SoundOutput != null)
            {
                bool result = SoundOutput.PlaySound(username, sound);

                if (!result)
                    OutputError(DateTime.Now, "Sound output", $"Couldn't play sound for user: {username}");
            }
        }

        public void GetTextMessageFromUser(string username, string message) => TextMessageOutput?.OutputMessage(DateTime.Now, username, message);

        public void GetVolume()
        {
            if (SoundOutput != null && VolumeGetter != null)
            {
                string username = VolumeGetter.GetUserForGetVolume();
                bool result = SoundOutput.GetPlayerVolume(username, out float volume);

                if (!result)
                    OutputError(DateTime.Now, "Sound output", $"Couldn't get volume for user: {username}");
                else
                    VolumeOutputter.OutputVolume(volume);
            }
        }

        public void Leave()
        {
            if (ConnectionHandler != null && ConnectionHandler.IsConnected)
                ConnectionHandler.Leave();

            ReturnToStartState();
        }

        public void Mute()
        {
            if (SoundOutput != null && MuteForm != null)
            {
                string username = MuteForm.GetUserForMute();
                bool result = SoundOutput.Mute(username);

                if (!result)
                    OutputError(DateTime.Now, "Sound output", $"Couldn't mute user: {username}");
            }
        }

        public void RemoveUser(string name)
        {
            if (SoundOutput != null)
            {
                bool result = SoundOutput.DeletePlayer(name);

                if (!result)
                    OutputError(DateTime.Now, "Sound output", $"Couldn't remove audioplayer for user: {name}");
            }

            UserList?.RemoveUser(name);
            UserLeaveHandler?.NotifyUserLeft(DateTime.Now, name);
        }

        public void SendTextMessage()
        {
            if (TextSender != null && TextSender.IsConnected && TextMessageInput != null)
            {
                string message = TextMessageInput.GetEnteredTextMessage();
                bool verificationResult = VerifyMessage(message);

                if (!verificationResult)
                    return;

                bool transformationResult = TransformMessage(ref message);

                if (!transformationResult)
                    return;

                TextSender.SendTextMessage(message);
            }
        }

        public void SetVolume()
        {
            if (SoundOutput != null && VolumeSetter != null)
            {
                string username = VolumeSetter.GetUserForSetVolume();
                float volume = VolumeSetter.GetNewVolumeForSetVolume();
                bool result = SoundOutput.SetPlayerVolume(username, volume);

                if (!result)
                    OutputError(DateTime.Now, "Sound output", $"Couldn't set volume for user: {username}");
            }
        }

        public void HandleDisconnect(string message)
        {
            ReturnToStartState();

            OutputError(DateTime.Now, "Service connector", message);
        }
    }
}