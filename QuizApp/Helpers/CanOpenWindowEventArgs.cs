using System;

namespace QuizApp.Helpers
{
    public class CanOpenWindowEventArgs : EventArgs
    {
        public string WindowName { get; set; }
        public bool CanOpen { get; set; }

        public CanOpenWindowEventArgs(string windowName, bool canOpen)
        {
            WindowName = windowName;
            CanOpen = canOpen;
        }
    }
}
