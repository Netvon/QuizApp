using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using QuizApp.Helpers;
using QuizApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuizApp.ViewModel
{
    public class EditorViewModel : ViewModelBase
    {
        QuizViewModel _selectedQuiz;
        IRepository<Quiz> _quizRepo;
        IRepository<Question> _questionRepo;
        IRepository<Category> _categoryRepo;
        Visibility _loadingVisibility;
        int _selectedTabIndex;

        public ObservableCollection<QuizViewModel> AllQuizes { get; set; }
        public ObservableCollection<QuestionViewModel> AllQuestions { get; set; }

        public RelayCommand RemoveQuizCommand { get; set; }
        public RelayCommand AddQuizCommand { get; set; }

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
            }
        }

        public int SelectedTabIndex
        {
            get
            {
                return _selectedTabIndex;
            }
            set
            {
                _selectedTabIndex = value;
                RaisePropertyChanged();
            }
        }

        readonly INotificationService _notificationService;
        readonly IWindowService _windowService;

        public EditorViewModel(IRepository<Quiz> quizRepo,
                               IRepository<Category> categoryRepo,
                               IRepository<Question> questionRepo,
                               INotificationService notificationService,
                               IWindowService windowService)
        {
            _windowService = windowService;
            _notificationService = notificationService;
            _quizRepo = quizRepo;
            _questionRepo = questionRepo;
            _categoryRepo = categoryRepo;

            notificationService.OnStartedLoading += NotificationService_OnLoadingChanged;
            notificationService.OnStoppedLoading += NotificationService_OnLoadingChanged;

            LoadingVisibility = Visibility.Hidden;

            RemoveQuizCommand = new RelayCommand(OnRemoveQuiz, CanRemoveQuiz);
            AddQuizCommand = new RelayCommand(OnAddQuiz);

            AllQuizes = new ObservableCollection<QuizViewModel>();
            AllQuestions = new ObservableCollection<QuestionViewModel>();

            foreach (var quiz in _quizRepo.GetAllItems())
            {
                AllQuizes.Add(new QuizViewModel(quiz, _quizRepo, _questionRepo, _categoryRepo, notificationService));
            }

            foreach (var question in _questionRepo.GetAllItems())
            {
                AllQuestions.Add(new QuestionViewModel(question, _questionRepo, _categoryRepo));
            }

            SelectedQuiz = new QuizViewModel(new Quiz() { Questions = new List<QuizQuestion>() }, _quizRepo, _questionRepo, _categoryRepo, notificationService);
            //SelectedTabIndex = 1;
        }

        void OnAddQuiz()
        {
            SelectedQuiz = new QuizViewModel(new Quiz() { Questions = new List<QuizQuestion>() }, _quizRepo, _questionRepo, _categoryRepo, _notificationService);
            AllQuizes.Add(SelectedQuiz);
        }

        bool CanRemoveQuiz()
        {
            return SelectedQuiz.RemoveQuizCommand.CanExecute(null);
        }

        void OnRemoveQuiz()
        {
            if(_windowService.AskConfirmation("Wil je deze Quiz echt verwijderen?", "EditorView"))
            {
                SelectedQuiz.RemoveQuizCommand.Execute(null);
                AllQuizes.Remove(SelectedQuiz);
                SelectedQuiz = new QuizViewModel(new Quiz() { Questions = new List<QuizQuestion>() }, _quizRepo, _questionRepo, _categoryRepo, _notificationService);
                RemoveQuizCommand.RaiseCanExecuteChanged();
            }
            
        }

        void NotificationService_OnLoadingChanged(object sender, LoadingNotificationEventArgs e)
        {
            if(e.Status == LoadingNotificationEventArgs.LoadingStatus.Ended)
                LoadingVisibility = Visibility.Hidden;
            else
                LoadingVisibility = Visibility.Visible;
        }
    }
}
