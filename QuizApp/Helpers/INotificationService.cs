using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Helpers
{
    public interface INotificationService
    {
        event EventHandler<LoadingNotificationEventArgs> OnStartedLoading;
        event EventHandler<LoadingNotificationEventArgs> OnStoppedLoading;
        event EventHandler<MessageNotificationEventArgs> OnNewDisplayMessage;

        void StartLoading(object token);
        void StopLoading(object token);

        void DisplayMessage(object token, string message);
    }
}
