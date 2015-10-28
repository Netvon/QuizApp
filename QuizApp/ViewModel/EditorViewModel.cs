using GalaSoft.MvvmLight;
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

        public ObservableCollection<QuizViewModel> AllQuizes { get; set; }
        public ObservableCollection<QuestionViewModel> AllQuestions { get; set; }
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
                RaisePropertyChanged();
            }
        }

        public EditorViewModel(IRepository<Quiz> quizRepo,
                               IRepository<Category> categoryRepo,
                               IRepository<Question> questionRepo,
                               INotificationService notificationService)
        {
            _quizRepo = quizRepo;
            _questionRepo = questionRepo;
            _categoryRepo = categoryRepo;

            notificationService.OnStartedLoading += NotificationService_OnStartedLoading;
            notificationService.OnStoppedLoading += NotificationService_OnStoppedLoading;

            LoadingVisibility = Visibility.Hidden;

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
        }

        void NotificationService_OnStoppedLoading(object sender, LoadingNotificationEventArgs e)
        {
            LoadingVisibility = Visibility.Hidden;
        }

        void NotificationService_OnStartedLoading(object sender, LoadingNotificationEventArgs e)
        {
            LoadingVisibility = Visibility.Visible;
        }
    }
}
