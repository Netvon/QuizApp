using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using QuizApp.Helpers;
using QuizApp.Model;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;
using System.Collections.Generic;
using System.Windows;

namespace QuizApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        QuizViewModel _selectedQuiz;
        IWindowService _windowService;
        Visibility _loadingVisibility;

        public RelayCommand OpenEditorCommand { get; set; }
        public RelayCommand OpenGameCommand { get; set; }

        public Visibility LoadingVisibility
        {
            get
            {
                return _loadingVisibility;
            }
            set
            {
                _loadingVisibility = value;
                RaisePropertyChanged();
            }
        }

        public QuizViewModel SelectedQuiz
        {
            get
            {
                return _selectedQuiz;
            }
            set
            {
                _selectedQuiz = value;
                RaisePropertyChanged("SelectedQuiz");
                OpenGameCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IWindowService windowService, INotificationService notificationService)
        {
            _windowService = windowService;
            _windowService.OnCanOpenWindowChanged += WindowService_OnCanOpenWindowChanged;

            LoadingVisibility = Visibility.Hidden;

            OpenEditorCommand = new RelayCommand(OnOpenEditor, CanOpenEditor);
            OpenGameCommand = new RelayCommand(OnOpenGame, CanOpenGame);
        }

        bool CanOpenGame()
        {
            var quizRepo = SimpleIoc.Default.GetInstance<IRepository<Quiz>>();
            return _windowService.CanOpenWindow("GameView") && SelectedQuiz != null && SelectedQuiz.ExistsInDatabase;
        }

        void OnOpenGame()
        {
            var quizRepo = SimpleIoc.Default.GetInstance<IRepository<Quiz>>();
            var questionRepo = SimpleIoc.Default.GetInstance<IRepository<Question>>();
            var categoryRepo = SimpleIoc.Default.GetInstance<IRepository<Category>>();
            var noti = SimpleIoc.Default.GetInstance<INotificationService>();
            var win = SimpleIoc.Default.GetInstance<IWindowService>();

            _windowService.OpenWindow("GameView", new GameViewModel(SelectedQuiz, quizRepo, questionRepo, categoryRepo, noti, win));
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