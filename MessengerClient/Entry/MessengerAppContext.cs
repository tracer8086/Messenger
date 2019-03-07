using System;
using System.Windows.Forms;
using MessengerClient.GUI.Classes;

namespace MessengerClient.Entry
{
    class MessengerAppContext : ApplicationContext
    {
        public EnterForm EnterForm { get; }
        public ChatForm ChatForm { get; }
        public ErrorForm ErrorForm { get; }

        public MessengerAppContext(EnterForm enterForm, ChatForm chatForm, ErrorForm errorForm)
        {
            EnterForm = enterForm;
            ChatForm = chatForm;
            ErrorForm = errorForm;

            EnterForm.FormClosed += CloseEnterForm;
            ChatForm.FormClosed += CloseChatForm;

            EnterForm.Show();
            ChatForm.Hide();
            ErrorForm.Hide();
        }

        private void CloseEnterForm(object sender, EventArgs eventArgs)
        {
            ChatForm.FormClosed -= CloseChatForm;
            ErrorForm.Close();
            ChatForm.Close();

            Application.ExitThread();
        }

        private void CloseChatForm(object sender, EventArgs eventArgs)
        {
            EnterForm.FormClosed -= CloseEnterForm;
            ErrorForm.Close();
            EnterForm.Close();

            Application.ExitThread();
        }
    }
}