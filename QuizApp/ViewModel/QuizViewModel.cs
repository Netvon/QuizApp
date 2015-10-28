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
            }
        }

        public QuestionViewModel SelectedQuestion { get; set; }
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
            //RemoveQuizCommand = new RelayCommand(OnRemoveQuiz, CanRemoveQuiz);
            //SaveQuizCommand = new RelayCommand(OnSaveQuiz, CanSaveQuiz);
            //AddQuestionCommand = new RelayCommand(OnAddQuestion, CanAddQuestion);
            //RemoveQuestionCommand = new RelayCommand(OnRemoveQuestion, CanRemoveQuestion);

        }

        public async void OnAddQuiz()
        {
            _quizRepo.Add(POCO);

            await _quizRepo.SaveAsync();
        }

        public bool CanAddQuiz()
        {
            if (Questions.Count < 2 || Questions.Count > 10)
                return false;

            var tst = _quizRepo.GetAllItems();

            bool isPresent = _quizRepo.AsQueryable().Any(q => q.QuizName == Name);

            return !isPresent;
        }

        //public async void OnAddQuestion()
        //{
        //    POCO.Questions.Add(new QuizQuestion() { Question = SelectedQuestion.POCO, Quiz = POCO });

        //    await _quizRepo.SaveAsync();
        //}
    }
}
