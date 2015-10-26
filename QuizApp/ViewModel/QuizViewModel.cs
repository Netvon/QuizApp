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
        IRepository<Quiz> _quizRepo;
        IRepository<Question> _questionRepo;
        IRepository<Category> _categoryRepo;

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

        public ObservableCollection<QuestionViewModel> Questions { get; set; }

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
        }
    }
}
