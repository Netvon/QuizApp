using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using QuizApp.Helpers;
using QuizApp.Model;
using System.Collections.ObjectModel;

namespace QuizApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        IWindowService _windowService;

        public IRepository<Question> QuestionRepo { get; set; }
        public IRepository<Category> CategoryRepo { get; set; }
        public IRepository<Quiz> QuizRepo { get; set; }

        public ObservableCollection<QuestionViewModel> Questions { get; set; }
        public ObservableCollection<CategoryViewModel> Categories { get; set; }
        public ObservableCollection<QuizViewModel> Quizes { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IWindowService windowService)
        {
            _windowService = windowService;

            QuestionRepo = SimpleIoc.Default.GetInstance<IRepository<Question>>();
            CategoryRepo = SimpleIoc.Default.GetInstance<IRepository<Category>>();
            QuizRepo = SimpleIoc.Default.GetInstance<IRepository<Quiz>>();

            Categories = new ObservableCollection<CategoryViewModel>();
            Questions = new ObservableCollection<QuestionViewModel>();
            Quizes = new ObservableCollection<QuizViewModel>();

            foreach (var cat in CategoryRepo.GetAllItems())
            {
                Categories.Add(new CategoryViewModel(cat, CategoryRepo));
            }

            foreach (var question in QuestionRepo.GetAllItems())
            {
                Questions.Add(new QuestionViewModel(question, QuestionRepo, CategoryRepo));
            }

            foreach (var quiz in QuizRepo.GetAllItems())
            {
                Quizes.Add(new QuizViewModel(quiz, QuizRepo, QuestionRepo, CategoryRepo));
            }
        }
    }
}