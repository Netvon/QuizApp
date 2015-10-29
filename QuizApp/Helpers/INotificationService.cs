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

        void StartLoading(object token);
        void StopLoading(object token);
    }
}
