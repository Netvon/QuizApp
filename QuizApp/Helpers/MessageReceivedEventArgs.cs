namespace QuizApp.Helpers
{
    public class MessageReceivedEventArgs
    {
        public string Message { get; internal set; }
        public object Sender { get; internal set; }
    }
}