using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using QuizApp.Helpers;
using QuizApp.Model;
using System.Collections.ObjectModel;
using System;

namespace QuizApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        IWindowService _windowService;

        public RelayCommand OpenEditorCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            _windowService.OnCanOpenWindowChanged += WindowService_OnCanOpenWindowChanged;

            OpenEditorCommand = new RelayCommand(OnOpenEditor, CanOpenEditor);
        }

        void WindowService_OnCanOpenWindowChanged(object sender, CanOpenWindowEventArgs e)
        {
            if(e.WindowName.Contains("EditorView"))
                OpenEditorCommand.RaiseCanExecuteChanged();
        }

        void OnOpenEditor()
        {
            _windowService.OpenWindow("EditorView");
            OpenEditorCommand.RaiseCanExecuteChanged();
        }

        bool CanOpenEditor()
        {
            return _windowService.CanOpenWindow("EditorView");
        }
    }
}