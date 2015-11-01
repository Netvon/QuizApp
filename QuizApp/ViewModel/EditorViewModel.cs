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
        String _inputAnswer;
        QuestionViewModel _selectedQuestion;
        IRepository<Quiz> _quizRepo;
        IRepository<Question> _questionRepo;
        IRepository<Category> _categoryRepo;
        Visibility _loadingVisibility;
        Visibility _notificationVisibility;
        Answer _selectedAnswer;
        string _notificationMessage;
        int _selectedTabIndex;

        public ObservableCollection<QuizViewModel> AllQuizes { get; set; }
        public ObservableCollection<QuestionViewModel> AllQuestions { get; set; }
        public ObservableCollection<CategoryViewModel> AllCategories { get; set; }

        public RelayCommand RemoveQuizCommand { get; set; }
        public RelayCommand AddQuizCommand { get; set; }

        public RelayCommand RemoveQuestionCommand { get; set; }

        public RelayCommand AddQuestionCommand { get; set; }

        public RelayCommand SaveQuestionCommand { get; set; }

        public RelayCommand AddAnswerCommand { get; set; }

        public RelayCommand RemoveAnswerCommand { get; set; }
        public RelayCommand CloseNotificationCommand { get; set; }

        public Boolean CanUncheck
        {
            get 
            {
                
                return SelectedQuestion.Answers.AsQueryable().Any(r => !r.IsCorrect);
            }
        }
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

        
        public QuestionViewModel SelectedQuestion
        {
            get
            {
                return _selectedQuestion;
            }
            set
            {
                if (value != null)
                {
                    _selectedQuestion = value;
                }
                
                RaisePropertyChanged("SelectedQuestion");               
                RaisePropertyChanged("CanEditQuestion");
                SaveQuestionCommand.RaiseCanExecuteChanged();
                RemoveQuestionCommand.RaiseCanExecuteChanged();
            }
        }

        public String InputAnswer
        {
            get
            {
                return _inputAnswer;
            }
            set
            {
                _inputAnswer = value;
                RaisePropertyChanged("InputAnswer");
                AddAnswerCommand.RaiseCanExecuteChanged();
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
                
            }
        }

        public Answer Answer
        {
            get
            {
                SaveQuestionCommand.RaiseCanExecuteChanged();
                return _selectedAnswer;
            }
            set
            {
                _selectedAnswer = value;
                RaisePropertyChanged("Answer");
                
                AddAnswerCommand.RaiseCanExecuteChanged();
                RemoveAnswerCommand.RaiseCanExecuteChanged();
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

        public bool CanEditQuestion
        {
            get
            {
                if (AllQuestions != null) {
                    return !string.IsNullOrEmpty(SelectedQuestion.Text); 
                }
                    
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

            AllQuizes = new ObservableCollection<QuizViewModel>();
            AllQuestions = new ObservableCollection<QuestionViewModel>();
            AllCategories = new ObservableCollection<CategoryViewModel>();

            notificationService.OnStartedLoading += NotificationService_OnLoadingChanged;
            notificationService.OnStoppedLoading += NotificationService_OnLoadingChanged;
            notificationService.OnNewDisplayMessage += NotificationService_OnNewDisplayMessage;
            notificationService.OnMessageReceived += NotificationService_OnMessageReceived;

            LoadingVisibility = Visibility.Hidden;
            NotificationVisibility = Visibility.Hidden;

            RemoveQuizCommand = new RelayCommand(OnRemoveQuiz, CanRemoveQuiz);
            AddQuizCommand = new RelayCommand(OnAddQuiz);
            RemoveQuestionCommand = new RelayCommand(OnRemoveQuestion, CanRemoveQuestion);
            AddQuestionCommand = new RelayCommand(OnAddQuestion);
            AddAnswerCommand = new RelayCommand(OnAddAnwer, CanAddAnswer);
            RemoveAnswerCommand = new RelayCommand(OnRemoveAnswer, CanRemoveAnswer);
            SaveQuestionCommand = new RelayCommand(OnSaveQuestion, CanSaveQuestion);
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
                    allQuizes.ForEach(q => AllQuizes.Add(q));
                    allQuestions.ForEach(q => AllQuestions.Add(q));
                    allCategories.ForEach(c => AllCategories.Add(c));

                    RaisePropertyChanged("AllQuizes");
                    RaisePropertyChanged("AllQuestions");
                    RaisePropertyChanged("AllCategories");
                    LoadingVisibility = Visibility.Hidden;
                });
            });            

            SelectedQuiz = new QuizViewModel(new Quiz() { Questions = new List<QuizQuestion>() }, _quizRepo, _questionRepo, _categoryRepo, notificationService);
            SelectedQuestion = new QuestionViewModel(new Question() { Answers = new List<Answer>(), Category = new Category() }, _questionRepo, _categoryRepo, _notificationService);
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

        void OnSaveQuestion()
        {
            Category cat = new Category();
            cat = SelectedQuestion.Category.POCO;
            foreach (var c in AllCategories)
            {
                if (c.POCO.Name.Equals(SelectedQuestion.CategoryName))
                {
                    cat = c.POCO;
                    break;
                }

            }
            SelectedQuestion.POCO.Category = cat;
            
            if (_questionRepo.GetAllItems().AsQueryable().Any(r => r.Text.Contains(SelectedQuestion.Text)))
            {
                _questionRepo.SaveAsync();
            }
            else
            {
                _questionRepo.Add(SelectedQuestion.POCO);
            }          
        }
        void OnAddQuiz()
        {
            SelectedQuiz = new QuizViewModel(new Quiz() { Questions = new List<QuizQuestion>() }, _quizRepo, _questionRepo, _categoryRepo, _notificationService);
            AllQuizes.Add(SelectedQuiz);
            RaisePropertyChanged("CanEditQuiz");
        }

        bool CanRemoveQuiz()
        {
            if (SelectedQuiz != null)
                return !string.IsNullOrEmpty(SelectedQuiz.Name);

            return false;
            //return SelectedQuiz.RemoveQuizCommand.CanExecute(null);
        }

        bool CanSaveQuestion()
        {
            return SelectedQuestion.CanAddQuestion();
            
        }

        bool CanRemoveQuestion()
        {
            return !string.IsNullOrEmpty(SelectedQuestion.Text);
        }

        bool CanRemoveAnswer()
        {
            return (SelectedQuestion.Answers.Contains(Answer) && SelectedQuestion.Answers.Count > 2);
        }

        bool CanAddAnswer()
        {       
            return (!string.IsNullOrEmpty(InputAnswer) && SelectedQuestion.Answers.Count() < 4);
        }

        void OnAddAnwer()
        {         
            Answer = new Answer() { AnswerText = InputAnswer, AnswerToQuestion = SelectedQuestion.POCO, IsCorrect = false, QuestionID = SelectedQuestion.POCO.QuestionID };
            SelectedQuestion.POCO.Answers.Add(Answer);
            _questionRepo.SaveAsync();
            SelectedQuestion.Answers.Add(Answer);
            Answer = new Answer() { AnswerText = "", AnswerToQuestion = SelectedQuestion.POCO, IsCorrect = false };
            InputAnswer = "";
            SaveQuestionCommand.RaiseCanExecuteChanged();
        }

        void OnRemoveAnswer()
        {
           
            SelectedQuestion.POCO.Answers.Remove(Answer);
            _questionRepo.SaveAsync();
            SelectedQuestion.Answers.Remove(Answer);
            Answer = new Answer();
            
            SaveQuestionCommand.RaiseCanExecuteChanged();
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

        void OnRemoveQuestion()
        {
            if (_windowService.AskConfirmation("Wil je deze vraag echt verwijderen?", "EditorView"))
            {
                SelectedQuestion.RemoveQuestionCommand.Execute(null);
                AllQuestions.Remove(SelectedQuestion);
                SelectedQuestion = new QuestionViewModel(new Question() { Text = "", Answers = new List<Answer>(), Category = new Category() }, _questionRepo, _categoryRepo, _notificationService);
                RemoveQuestionCommand.RaiseCanExecuteChanged();
            }
        }

        void OnAddQuestion()
        {
            SelectedQuestion = new QuestionViewModel(new Question() { Answers = new List<Answer>(), Category = new Category() }, _questionRepo, _categoryRepo, _notificationService);
            AllQuestions.Add(SelectedQuestion);
            RaisePropertyChanged("SelectedQuestion");
            RaisePropertyChanged("CanEditQuestion");
            RaisePropertyChanged("CanSaveQuestion");
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
