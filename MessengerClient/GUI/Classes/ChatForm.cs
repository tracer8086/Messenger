using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessengerClient.Controllers.Interfaces;
using MessengerClient.GUI.Interfaces;

namespace MessengerClient.GUI.Classes
{
    public partial class ChatForm : Form, ITextMessageOutput, IUserList, ITextMessageInput, IUserJoinHandler, IUserLeaveHandler
    {
        public ITextMessageController TextMessageController { get; set; }
        public IConnectionController ConnectionController { get; set; }
        public ISoundInputController SoundInputController { get; set; }
        public ISoundOutputController SoundOutputController { get; set; }

        public bool IsVisible => Visible;

        public ChatForm()
        {
            InitializeComponent();

            Show();

            // Chat window.
            chatWindow.Font = new Font("Arial", 10);
            chatWindow.ForeColor = Color.DarkBlue;
        }

        private void AddUserToList(string user) => userList.Items.Add(user, user, 0);

        private void RemoveUserFromList(string user) => userList.Items.RemoveByKey(user);

        private async Task SendMessage()
        {
            await Task.Run(() => TextMessageController?.SendTextMessage());

            messageBox.Clear();
        }

        private async Task LeaveChat() => await Task.Run(() => ConnectionController?.Leave());

        private async Task EnableMicrophone() => await Task.Run(() => SoundInputController?.EnableInput());

        private async Task DisableMicrophone() => await Task.Run(() => SoundInputController?.DisableInput());

        private async void sendButton_Click(object sender, EventArgs e) => await SendMessage();

        private void OutputMessageToChat(DateTime time, string sender, string message)
        {
            int temp = chatWindow.SelectionStart;

            chatWindow.SelectionStart = chatWindow.Text.Length;

            chatWindow.SelectedText = $"[{time:dd:MM:yyyy - HH:mm:ss}] ";

            chatWindow.SelectionFont = new Font("Arial Black", 10);
            chatWindow.SelectionColor = Color.Red;
            chatWindow.SelectedText = sender + ": ";

            chatWindow.SelectionFont = new Font("Arial", 10);
            chatWindow.SelectionColor = Color.Black;
            chatWindow.SelectedText = message + "\n";

            chatWindow.SelectionStart = temp;
        }

        private void OutputUserJoined(DateTime time, string username)
        {
            int temp = chatWindow.SelectionStart;

            chatWindow.SelectionStart = chatWindow.Text.Length;

            chatWindow.SelectedText = $"[{time:dd:MM:yyyy - HH:mm:ss}] ";

            chatWindow.SelectionFont = new Font("Arial Black", 10);
            chatWindow.SelectionColor = Color.Red;
            chatWindow.SelectedText = username + " ";

            chatWindow.SelectionFont = new Font("Arial", 10);
            chatWindow.SelectionColor = Color.Green;
            chatWindow.SelectedText = "has just joined the chat." + "\n";

            chatWindow.SelectionStart = temp;
        }

        private void OutputUserLeft(DateTime time, string username)
        {
            int temp = chatWindow.SelectionStart;

            chatWindow.SelectionStart = chatWindow.Text.Length;

            chatWindow.SelectedText = $"[{time:dd:MM:yyyy - HH:mm:ss}] ";

            chatWindow.SelectionFont = new Font("Arial Black", 10);
            chatWindow.SelectionColor = Color.Red;
            chatWindow.SelectedText = username + " ";

            chatWindow.SelectionFont = new Font("Arial", 10);
            chatWindow.SelectionColor = Color.OrangeRed;
            chatWindow.SelectedText = "has just left the chat." + "\n";

            chatWindow.SelectionStart = temp;
        }

        private void OutputClientJoined(DateTime time, string username)
        {
            int temp = chatWindow.SelectionStart;

            chatWindow.SelectionStart = chatWindow.Text.Length;

            chatWindow.SelectedText = $"[{time:dd:MM:yyyy - HH:mm:ss}] ";

            chatWindow.SelectionFont = new Font("Arial", 10);
            chatWindow.SelectionColor = Color.Green;
            chatWindow.SelectedText = "You have entered the chat as ";

            chatWindow.SelectionFont = new Font("Arial Black", 10);
            chatWindow.SelectionColor = Color.Red;
            chatWindow.SelectedText = username + ".\n";

            chatWindow.SelectionStart = temp;
        }

        public void OutputMessage(DateTime time, string sender, string message) => chatWindow.ControlInvoke(() => OutputMessageToChat(time, sender, message));

        public void ShowControl() => this.ControlInvoke(() => Show());

        public void HideControl() => this.ControlInvoke(() => Hide());

        public void AddUser(string user) => userList.ControlInvoke(() => userList.Items.Add(user, user, 0));

        public void RemoveUser(string user) => userList.ControlInvoke(() => userList.Items.RemoveByKey(user));

        private async void logOutButton_Click(object sender, EventArgs e) => await LeaveChat();

        public string GetEnteredTextMessage() => messageBox.Text;

        public void Clear() => userList.ControlInvoke(() => userList.Items.Clear());

        public void ClearChat() => chatWindow.ControlInvoke(() => chatWindow.Clear());

        private async void messageBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                await SendMessage();
        }

        private async void enableMicrophoneButton_Click(object sender, EventArgs e) => await EnableMicrophone();

        private async void disableMicrophoneButton_Click(object sender, EventArgs e) => await DisableMicrophone();

        private async void enableOutputButton_Click(object sender, EventArgs e) => await Task.Run(() => SoundOutputController?.EnableOutput());

        private async void disableOutputButton_Click(object sender, EventArgs e) => await Task.Run(() => SoundOutputController?.DisableOutput());

        private void ChatForm_FormClosed(object sender, FormClosingEventArgs e) => ConnectionController?.Leave();

        public void NotifyUserJoined(DateTime time, string username) => chatWindow.ControlInvoke(() => OutputUserJoined(time, username));

        public void NotifyUserLeft(DateTime time, string username) => chatWindow.ControlInvoke(() => OutputUserLeft(time, username));

        public void NotifyClientJoined(DateTime time, string username) => chatWindow.ControlInvoke(() => OutputClientJoined(time, username));
    }
}