using GalaSoft.MvvmLight.Command;
using QuizApp.Helpers;
using QuizApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.ViewModel
{
    public class QuizViewModel : POCOViewModel<Quiz>
    {
        #region Fields
        IRepository<Quiz> _quizRepo;
        IRepository<Question> _questionRepo;
        IRepository<Category> _categoryRepo;

        INotificationService _notificationService;

        QuestionViewModel _selectedDropdownQuestion;
        QuestionViewModel _selectedListQuestion;
        #endregion

        #region Properties


        public string Name
        {
            get
            {
                return POCO.QuizName;
            }
            set
            {
                POCO.QuizName = value;
                RaisePropertyChanged();
                AddQuizCommand.RaiseCanExecuteChanged();
            }
        }

        public QuestionViewModel SelectedDropdownQuestion
        {
            get
            {
                return _selectedDropdownQuestion;
            }
            set
            {
                _selectedDropdownQuestion = value;
                RaisePropertyChanged();
                AddQuestionCommand.RaiseCanExecuteChanged();
            }
        }

        public QuestionViewModel SelectedListQuestion
        {
            get
            {
                return _selectedListQuestion;
            }
            set
            {
                _selectedListQuestion = value;
                RaisePropertyChanged();
                RemoveQuestionCommand.RaiseCanExecuteChanged();
            }
        }

        public bool ExistsInDatabase
        {
            get
            {
                return _quizRepo.AsQueryable().Any(q => q.QuizName == Name);
            }
        }

        //public ObservableCollection<QuestionViewModel> AllQuestions { get; set; }

        #region Commands

        public RelayCommand AddQuizCommand { get; set; }
        public RelayCommand RemoveQuizCommand { get; set; }
        public RelayCommand SaveQuizCommand { get; set; }
        public RelayCommand AddQuestionCommand { get; set; }
        public RelayCommand RemoveQuestionCommand { get; set; }

        #endregion

        public ObservableCollection<QuestionViewModel> Questions { get; set; }
        #endregion

        public QuizViewModel(Quiz poco, IRepository<Quiz> quizRepo, IRepository<Question> questionRepo, IRepository<Category> categoryRepo, INotificationService notificationService)
            : base(poco)
        {
            _quizRepo = quizRepo;
            _questionRepo = questionRepo;
            _categoryRepo = categoryRepo;

            _notificationService = notificationService;

            Questions = new ObservableCollection<QuestionViewModel>();

            foreach (var item in poco.Questions)
            {
                Questions.Add(new QuestionViewModel(item.Question, _questionRepo, _categoryRepo, _notificationService));
            }

            //AllQuestions = new ObservableCollection<QuestionViewModel>();

            //foreach (var item in questionRepo.GetAllItems())
            //{
            //    AllQuestions.Add(new QuestionViewModel(item, questionRepo, categoryRepo));
            //}

            AddQuizCommand = new RelayCommand(OnAddQuiz, CanAddQuiz);
            RemoveQuizCommand = new RelayCommand(OnRemoveQuiz);
            SaveQuizCommand = new RelayCommand(OnSaveQuiz);
            AddQuestionCommand = new RelayCommand(OnAddQuestion, CanAddQuestion);
            RemoveQuestionCommand = new RelayCommand(OnRemoveQuestion, CanRemoveQuestion);

        }

        async void OnAddQuiz()
        {
            _quizRepo.Add(POCO);

            _notificationService.StartLoading("QuizViewModel");
            await _quizRepo.SaveAsync();
            AddQuizCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged("ExistsInDatabase");
            _notificationService.StopLoading("QuizViewModel");
            _notificationService.DisplayMessage("QuizViewModel", "Quiz \"" + Name + "\" toegevoegd.");
        }

        bool CanAddQuiz()
        {
            if (Questions.Count < 2 || Questions.Count > 10 || string.IsNullOrWhiteSpace(Name))
                return false;

            var tst = _quizRepo.GetAllItems();

            bool isPresent = _quizRepo.AsQueryable().Any(q => q.QuizName == Name);

            return !isPresent;
        }

        async void OnAddQuestion()
        {
            if (SelectedDropdownQuestion == null)
                return;

            POCO.Questions.Add(new QuizQuestion() { Question = SelectedDropdownQuestion.POCO, Quiz = POCO, QuestionID = SelectedDropdownQuestion.POCO.QuestionID, QuizName = POCO.QuizName });
            Questions.Add(SelectedDropdownQuestion);
            SelectedDropdownQuestion = null;
            AddQuizCommand.RaiseCanExecuteChanged();

            if (!ExistsInDatabase)
                return;

            _notificationService.StartLoading("QuizViewModel");
            await _quizRepo.SaveAsync();
            _notificationService.StopLoading("QuizViewModel");
        }

        bool CanAddQuestion()
        {
            if (SelectedDropdownQuestion == null)
                return false;

            if (!_questionRepo.AsQueryable().Any(q => q.QuestionID == SelectedDropdownQuestion.POCO.QuestionID))
                return false;

            if (Questions.Count >= 10 ||
                Questions.Any(q => q.POCO.QuestionID == SelectedDropdownQuestion.POCO.QuestionID))
                return false;

            return true;
        }

        async void OnRemoveQuestion()
        {
            var remove = POCO.Questions.FirstOrDefault(q => q.QuestionID == SelectedListQuestion.POCO.QuestionID);
            POCO.Questions.Remove(remove);
            Questions.Remove(SelectedListQuestion);
            SelectedListQuestion = null;
            AddQuizCommand.RaiseCanExecuteChanged();

            if (!ExistsInDatabase)
                return;

            _notificationService.StartLoading("QuizViewModel");
            await _quizRepo.SaveAsync();
            _notificationService.StopLoading("QuizViewModel");
        }

        bool CanRemoveQuestion()
        {
            if (POCO.Questions.Count <= 2)
                return false;

            return SelectedListQuestion != null;
        }

        async void OnSaveQuiz()
        {
            _notificationService.StartLoading("QuizViewModel");
            await _quizRepo.SaveAsync();
            _notificationService.StopLoading("QuizViewModel");
        }

        async void OnRemoveQuiz()
        {
            _quizRepo.Remove(POCO);

            _notificationService.StartLoading("QuizViewModel");
            await _quizRepo.SaveAsync();
            _notificationService.StopLoading("QuizViewModel");
        }

        public async Task AddGivenAnswerToQuestion(QuestionViewModel question, Answer answer)
        {
            var qq = POCO.Questions.FirstOrDefault(q => q.Question.QuestionID == question.POCO.QuestionID);
            //qq.GivenAnswer = answer;
            qq.GivenAnswerText = answer.AnswerText;

            await _quizRepo.SaveAsync();
        }
    }
}
