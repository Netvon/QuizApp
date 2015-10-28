using GalaSoft.MvvmLight.Command;
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

        public ObservableCollection<QuestionViewModel> AllQuestions { get; set; }

        #region Commands

        public RelayCommand AddQuizCommand { get; set; }
        public RelayCommand RemoveQuizCommand { get; set; }
        public RelayCommand SaveQuizCommand { get; set; }
        public RelayCommand AddQuestionCommand { get; set; }
        public RelayCommand RemoveQuestionCommand { get; set; }

        #endregion

        public ObservableCollection<QuestionViewModel> Questions { get; set; }
        #endregion

        public QuizViewModel(Quiz poco, IRepository<Quiz> quizRepo, IRepository<Question> questionRepo, IRepository<Category> categoryRepo) 
            : base(poco)
        {
            _quizRepo = quizRepo;
            _questionRepo = questionRepo;
            _categoryRepo = categoryRepo;

            Questions = new ObservableCollection<QuestionViewModel>();

            foreach (var item in poco.Questions)
            {
                Questions.Add(new QuestionViewModel(item.Question, _questionRepo, _categoryRepo));
            }

            AllQuestions = new ObservableCollection<QuestionViewModel>();

            foreach (var item in questionRepo.GetAllItems())
            {
                AllQuestions.Add(new QuestionViewModel(item, questionRepo, categoryRepo));
            }

            AddQuizCommand = new RelayCommand(OnAddQuiz, CanAddQuiz);
            RemoveQuizCommand = new RelayCommand(OnRemoveQuiz);
            SaveQuizCommand = new RelayCommand(OnSaveQuiz);
            AddQuestionCommand = new RelayCommand(OnAddQuestion, CanAddQuestion);
            RemoveQuestionCommand = new RelayCommand(OnRemoveQuestion, CanRemoveQuestion);

        }

        async void OnAddQuiz()
        {
            _quizRepo.Add(POCO);

            await _quizRepo.SaveAsync();
        }

        bool CanAddQuiz()
        {
            if (Questions.Count < 2 || Questions.Count > 10)
                return false;

            var tst = _quizRepo.GetAllItems();

            bool isPresent = _quizRepo.AsQueryable().Any(q => q.QuizName == Name);

            return !isPresent;
        }

        async void OnAddQuestion()
        {
            if (SelectedDropdownQuestion == null)
                return;

            POCO.Questions.Add(new QuizQuestion() { Question = SelectedDropdownQuestion.POCO, Quiz = POCO });
            Questions.Add(SelectedDropdownQuestion);
            SelectedDropdownQuestion = null;

            await _quizRepo.SaveAsync();
        }

        bool CanAddQuestion()
        {
            if (SelectedDropdownQuestion == null)
                return false;

            if (Questions.Count >= 10 || POCO.Questions.Any(q => q.QuestionID == SelectedDropdownQuestion.POCO.QuestionID))
                return false;

            return true;
        }

        async void OnRemoveQuestion()
        {
            POCO.Questions.Remove(new QuizQuestion() { Question = SelectedListQuestion.POCO, Quiz = POCO });
            Questions.Remove(SelectedListQuestion);
            SelectedListQuestion = null;

            await _quizRepo.SaveAsync();
        }

        bool CanRemoveQuestion()
        {
            if (POCO.Questions.Count <= 2)
                return false;

            return SelectedListQuestion != null;
        }

        async void OnSaveQuiz()
        {
            await _quizRepo.SaveAsync();
        }

        async void OnRemoveQuiz()
        {
            _quizRepo.Remove(POCO);

            await _quizRepo.SaveAsync();
        }
    }
}
