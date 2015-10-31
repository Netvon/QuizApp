using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using QuizApp.Helpers;
using QuizApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuizApp.ViewModel
{
    public class GameViewModel : ViewModelBase
    {
        #region Fields
        QuizViewModel _quiz;
        QuestionViewModel _currentQuestion;
        IRepository<Quiz> _quizRepo;
        Queue<QuestionViewModel> _questions;
        int _correctCount;
        int _inCorrectCount;
        int _currentQuestionCount;
        Visibility _loadingVisibility;
        Visibility _doneVisibility;
        #endregion

        #region Commands
        public RelayCommand<Answer> SelectAnswerCommand { get; set; }
        public RelayCommand CloseWindowCommand { get; set; }
        #endregion

        #region Properties
        public QuizViewModel Quiz
        {
            get
            {
                return _quiz;
            }

            set
            {
                if (_quiz == value)
                {
                    return;
                }

                _quiz = value;
                RaisePropertyChanged();
            }
        }
        public QuestionViewModel CurrentQuestion
        {
            get
            {
                return _currentQuestion;
            }

            set
            {
                if (_currentQuestion == value)
                {
                    return;
                }

                _currentQuestion = value;
                RaisePropertyChanged();
            }
        }
        public int TotalQuestions
        {
            get
            {
                return _quiz.Questions.Count;
            }
        }
        public int RemainingQuestion
        {
            get
            {
                return TotalQuestions - _questions.Count;
            }
        }
        public int CorrectCount
        {
            get
            {
                return _correctCount;
            }
        }
        public int IncorrectCount
        {
            get
            {
                return _inCorrectCount;
            }
        }
        public bool HasQuizEnded
        {
            get
            {
                return _questions.Count <= 0;
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
        public Visibility DoneVisibility
        {
            get
            {
                return _doneVisibility;
            }
            set
            {
                _doneVisibility = value;
                RaisePropertyChanged();
            }
        }
        public int CurrentQuestionCount
        {
            get
            {
                return _currentQuestionCount;
            }
            set
            {
                _currentQuestionCount = value;
                RaisePropertyChanged();
            }
        }

        readonly IWindowService _windowService;
        #endregion

        public GameViewModel(QuizViewModel quiz, 
            IRepository<Quiz> quizRepo, 
            IRepository<Question> questionRepo, 
            IRepository<Category> categoryRepo, 
            INotificationService notificationService,
            IWindowService windowService)
        {
            _windowService = windowService;
            _quizRepo = quizRepo;
            _questions = new Queue<QuestionViewModel>();

            _quiz = quiz;//new QuizViewModel(_quizRepo.AsQueryable().First(), quizRepo, questionRepo, categoryRepo, notificationService);

            SelectAnswerCommand = new RelayCommand<Answer>(OnSelectAnswer);
            CloseWindowCommand = new RelayCommand(OnCloseWindow);

            foreach (var question in _quiz.Questions.Shuffle(new Random()))
            {
                _questions.Enqueue(question);
            }

            CurrentQuestion = _questions.Peek();
            CurrentQuestionCount = 1;
            LoadingVisibility = Visibility.Hidden;
            DoneVisibility = Visibility.Hidden;
        }

        void OnCloseWindow()
        {
            _windowService.CloseWindow("GameView");
        }

        async void OnSelectAnswer(Answer obj)
        {
            LoadingVisibility = Visibility.Visible;

            if (obj.IsCorrect)
                _correctCount++;
            else
                _inCorrectCount++;

            RaisePropertyChanged("CorrectCount");
            RaisePropertyChanged("IncorrectCount");

            await _quiz.AddGivenAnswerToQuestion(_questions.Dequeue(), obj);

            if (_questions.Count > 0)
            {
                CurrentQuestion = _questions.Peek();
                CurrentQuestionCount++;
                RaisePropertyChanged("CurrentQuestion");
            }
            else
            {
                DoneVisibility = Visibility.Visible;
                RaisePropertyChanged("HasQuizEnded");
            }

            LoadingVisibility = Visibility.Hidden;

        }
    }
}
