using System;

namespace QuizApp.Helpers
{
    public class LoadingNotificationEventArgs : EventArgs
    {
        public object Token { get; set; }
        public LoadingStatus Status { get; set; }

        public enum LoadingStatus
        {
            Started,
            Ended
        }
    }
}