using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Helpers
{
    public class WpfNotificationService : INotificationService
    {
        public event EventHandler<LoadingNotificationEventArgs> OnStartedLoading;
        public event EventHandler<LoadingNotificationEventArgs> OnStoppedLoading;

        public void StartLoading(object token)
        {
            var onStartedLoading = OnStartedLoading;
            onStartedLoading(this, new LoadingNotificationEventArgs() { Token = token, Status = LoadingNotificationEventArgs.LoadingStatus.Started });
        }

        public void StopLoading(object token)
        {
            var onStoppedLoading = OnStoppedLoading;
            onStoppedLoading(this, new LoadingNotificationEventArgs() { Token = token, Status = LoadingNotificationEventArgs.LoadingStatus.Ended });
        }
    }
}
