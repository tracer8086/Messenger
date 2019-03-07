using System;
using System.Drawing;
using System.Windows.Forms;
using MessengerClient.GUI.Interfaces;

namespace MessengerClient.GUI.Classes
{
    public partial class ErrorForm : Form, IErrorMessageOutput
    {
        public ErrorForm()
        {
            InitializeComponent();

            Show();

            // Error text box.
            errorTextBox.Font = new Font("Arial", 10);
            errorTextBox.ForeColor = Color.Red;

            ControlBox = false;
        }

        private void OutputErrorMessageToTextBox(DateTime time, string sender, string message)
        {
            int temp = errorTextBox.SelectionStart;

            errorTextBox.SelectionStart = errorTextBox.Text.Length;

            errorTextBox.SelectionColor = Color.DarkBlue;
            errorTextBox.SelectedText = $"[{time:dd:MM:yyyy - HH:mm:ss}] ";

            errorTextBox.SelectionFont = new Font("Arial Black", 10);
            errorTextBox.SelectionColor = Color.Red;
            errorTextBox.SelectedText = "Error from " + sender + ": ";

            errorTextBox.SelectionFont = new Font("Arial", 10);
            errorTextBox.SelectionColor = Color.Black;
            errorTextBox.SelectedText = message + "\n";

            errorTextBox.SelectionStart = temp;
        }

        public bool IsVisible => Visible;

        public void HideControl() => this.ControlInvoke(() => Hide());

        public void OutputErrorMessage(DateTime time, string sender, string message) => this.ControlInvoke(() => OutputErrorMessageToTextBox(time, sender, message));

        public void ShowControl() => this.ControlInvoke(() => Show());
    }
}