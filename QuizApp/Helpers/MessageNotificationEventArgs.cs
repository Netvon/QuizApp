using System;

namespace QuizApp.Helpers
{
    public class MessageNotificationEventArgs : EventArgs
    {
        public object Token { get; set; }
        public string Message { get; set; }
    }
}