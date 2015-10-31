using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using QuizApp.Helpers;
using QuizApp.Model;
using System.Collections.ObjectModel;
using System;
using System.Linq;

namespace QuizApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        IWindowService _windowService;

        public RelayCommand OpenEditorCommand { get; set; }
        public RelayCommand OpenGameCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            _windowService.OnCanOpenWindowChanged += WindowService_OnCanOpenWindowChanged;

            OpenEditorCommand = new RelayCommand(OnOpenEditor, CanOpenEditor);
            OpenGameCommand = new RelayCommand(OnOpenGame, CanOpenGame);
        }

        bool CanOpenGame()
        {
            return _windowService.CanOpenWindow("GameView");
        }

        void OnOpenGame()
        {
            var quizRepo = SimpleIoc.Default.GetInstance<IRepository<Quiz>>();
            var questionRepo = SimpleIoc.Default.GetInstance<IRepository<Question>>();
            var categoryRepo = SimpleIoc.Default.GetInstance<IRepository<Category>>();
            var noti = SimpleIoc.Default.GetInstance<INotificationService>();

            var qvm = new QuizViewModel(quizRepo.AsQueryable().First(), quizRepo, questionRepo, categoryRepo, noti);

            _windowService.OpenWindow("GameView", new GameViewModel(qvm, quizRepo, questionRepo, categoryRepo, noti));
            OpenGameCommand.RaiseCanExecuteChanged();
        }

        void WindowService_OnCanOpenWindowChanged(object sender, CanOpenWindowEventArgs e)
        {
            if(e.WindowName.Contains("EditorView"))
                OpenEditorCommand.RaiseCanExecuteChanged();

            if (e.WindowName.Contains("GameView"))
                OpenGameCommand.RaiseCanExecuteChanged();
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