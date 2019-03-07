using System;
using System.Windows.Forms;

namespace MessengerClient.GUI.Classes
{
    public static class ControlInvokation
    {
        public static void ControlInvoke(this Control control, Action action)
        {
            if (control.InvokeRequired && !control.Disposing)
                control.Invoke(action);
            else
                action?.Invoke();
        }

        public static void ControlInvoke(this Form form, Action action)
        {
            if (form.InvokeRequired && !form.Disposing)
                form.Invoke(action);
            else
                action?.Invoke();
        }
    }
}