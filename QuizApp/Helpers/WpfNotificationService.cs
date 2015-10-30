using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Helpers
{
    public class WpfNotificationService : INotificationService
    {
        public event EventHandler<MessageNotificationEventArgs> OnNewDisplayMessage;
        public event EventHandler<LoadingNotificationEventArgs> OnStartedLoading;
        public event EventHandler<LoadingNotificationEventArgs> OnStoppedLoading;
        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

        public void DisplayMessage(object token, string message)
        {
            var onNewDisplayMessage = OnNewDisplayMessage;
            onNewDisplayMessage(this, new MessageNotificationEventArgs() { Token = token, Message = message });
        }

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

        public void SendMessage(object sender, string message)
        {
            var onMessageReceived = OnMessageReceived;
            onMessageReceived(this, new MessageReceivedEventArgs() { Sender = sender, Message = message });
        }
    }
}
