using System;

namespace QuizApp.Helpers
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public string Message { get; internal set; }
        public object Sender { get; internal set; }
    }
}