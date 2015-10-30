using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
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
        Visibility _notificationVisibility;
        string _notificationMessage;
        int _selectedTabIndex;

        public ObservableCollection<QuizViewModel> AllQuizes { get; set; }
        public ObservableCollection<QuestionViewModel> AllQuestions { get; set; }
        public ObservableCollection<CategoryViewModel> AllCategories { get; set; }

        public RelayCommand RemoveQuizCommand { get; set; }
        public RelayCommand AddQuizCommand { get; set; }
        public RelayCommand CloseNotificationCommand { get; set; }

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

        public Visibility NotificationVisibility
        {
            get
            {
                return _notificationVisibility;
            }
            set
            {
                _notificationVisibility = value;
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
                RaisePropertyChanged("CanEditQuiz");
                RemoveQuizCommand.RaiseCanExecuteChanged();
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

        public string NotificationMessage
        {
            get
            {
                return _notificationMessage;
            }
            set
            {
                _notificationMessage = value;
                RaisePropertyChanged();
            }
        }

        public bool CanEditQuiz
        {
            get
            {
                if(AllQuizes != null)
                    return AllQuizes.Contains(SelectedQuiz);

                return false;
            }
        }

        public CategoryViewModel NewCategory { get; set; }

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
            notificationService.OnNewDisplayMessage += NotificationService_OnNewDisplayMessage;
            notificationService.OnMessageReceived += NotificationService_OnMessageReceived;

            LoadingVisibility = Visibility.Hidden;
            NotificationVisibility = Visibility.Hidden;

            RemoveQuizCommand = new RelayCommand(OnRemoveQuiz, CanRemoveQuiz);
            AddQuizCommand = new RelayCommand(OnAddQuiz);
            CloseNotificationCommand = new RelayCommand(OnCloseNotification);

            Task.Run(() =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    LoadingVisibility = Visibility.Visible;
                });

                var allQuizes = new List<QuizViewModel>();
                var allQuestions = new List<QuestionViewModel>();
                var allCategories = new List<CategoryViewModel>();

                foreach (var quiz in _quizRepo.GetAllItems())
                {                
                    allQuizes.Add(new QuizViewModel(quiz, _quizRepo, _questionRepo, _categoryRepo, notificationService));
                }

                foreach (var question in _questionRepo.GetAllItems())
                {
                    allQuestions.Add(new QuestionViewModel(question, _questionRepo, _categoryRepo, _notificationService));
                }

                foreach (var categorie in _categoryRepo.GetAllItems())
                {
                    allCategories.Add(new CategoryViewModel(categorie, _categoryRepo, _notificationService));
                }

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    AllQuizes = new ObservableCollection<QuizViewModel>(allQuizes);
                    AllQuestions = new ObservableCollection<QuestionViewModel>(allQuestions);
                    AllCategories = new ObservableCollection<CategoryViewModel>(allCategories);
                    RaisePropertyChanged("AllQuizes");
                    RaisePropertyChanged("AllQuestions");
                    RaisePropertyChanged("AllCategories");
                    LoadingVisibility = Visibility.Hidden;
                });
            });            

            SelectedQuiz = new QuizViewModel(new Quiz() { Questions = new List<QuizQuestion>() }, _quizRepo, _questionRepo, _categoryRepo, notificationService);
            NewCategory = new CategoryViewModel(new Category(), _categoryRepo, _notificationService);
            //SelectedTabIndex = 1;
        }

        void NotificationService_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if ((string)e.Sender == "CategoryViewModel" && e.Message == "CatAdded")
            {
                AllCategories.Add(new CategoryViewModel(new Category() { Name = NewCategory.Name }, _categoryRepo, _notificationService));
                RaisePropertyChanged("AllCategories");
            }
        }

        void OnCloseNotification()
        {
            NotificationVisibility = Visibility.Hidden;
        }

        void OnAddQuiz()
        {
            SelectedQuiz = new QuizViewModel(new Quiz() { Questions = new List<QuizQuestion>() }, _quizRepo, _questionRepo, _categoryRepo, _notificationService);
            AllQuizes.Add(SelectedQuiz);
            RaisePropertyChanged("CanEditQuiz");
        }

        bool CanRemoveQuiz()
        {
            return !string.IsNullOrEmpty(SelectedQuiz.Name);
            //return SelectedQuiz.RemoveQuizCommand.CanExecute(null);
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

        void NotificationService_OnNewDisplayMessage(object sender, MessageNotificationEventArgs e)
        {
            if(e.Token as string == "QuizViewModel")
            {
                NotificationVisibility = Visibility.Visible;
                NotificationMessage = e.Message;
            }
            
        }
    }
}
