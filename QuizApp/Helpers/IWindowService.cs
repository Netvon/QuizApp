using GalaSoft.MvvmLight;
using System;

namespace QuizApp.Helpers
{
    public interface IWindowService
    {
        event EventHandler<CanOpenWindowEventArgs> OnCanOpenWindowChanged;
        bool CanOpenWindow(string name);
        void OpenWindow(string name);
        void OpenWindow(string name, ViewModelBase viewModel);
        bool AskConfirmation(string promt, string owner);
    }
}
