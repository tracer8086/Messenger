using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessengerClient.GUI.Interfaces;
using MessengerClient.Controllers.Interfaces;
using MessengerClient.GUI.Classes;

namespace MessengerClient
{
    public partial class EnterForm : Form, IEnterForm
    {
        public IConnectionController ConnectionController { get; set; }

        public EnterForm()
        {
            InitializeComponent();
        }

        private async Task EnterChat() => await Task.Run(() => ConnectionController?.Enter());

        private async Task EnterEvent()
        {
            enterButton.Enabled = false;
            nicknameTextBox.Enabled = false;

            await EnterChat();

            enterButton.Enabled = true;
            nicknameTextBox.Enabled = true;
        }

        public bool IsVisible => Visible;

        public string GetUserDataForEnter() => nicknameTextBox.Text;

        private async void enterButton_Click(object sender, EventArgs e) => await EnterEvent();

        public void ShowControl() => this.ControlInvoke(() => Show());

        public void HideControl() => this.ControlInvoke(() => Hide());

        private async void nicknameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                await EnterEvent();
        }
    }
}