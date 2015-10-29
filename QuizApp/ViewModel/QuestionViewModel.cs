using QuizApp.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.ViewModel
{
    public class QuestionViewModel : POCOViewModel<Question>
    {
        IRepository<Question> _questionRepo;
        IRepository<Category> _catRepo;

        CategoryViewModel _category;

        public RelayCommand AddQuestionCommand { get; set; }

        public string Text
        {
            get
            {
                return POCO.Text;
            }
            set
            {
                POCO.Text = value;
                RaisePropertyChanged();
            }
        }

        public CategoryViewModel Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Answer> Answers { get; set; }

        public QuestionViewModel(Question poco, IRepository<Question> questionRepo, IRepository<Category> categoryRepo)
            : base(poco)
        {
            _questionRepo = questionRepo;
            _catRepo = categoryRepo;
            Answers = new ObservableCollection<Answer>(poco.Answers);
            Category = new CategoryViewModel(poco.Category, _catRepo);

            AddQuestionCommand = new RelayCommand(OnAddQuestion, CanAddQuestion);
        }

        async void OnAddQuestion()
        {
           
            _questionRepo.Add(POCO);

            await _questionRepo.SaveAsync();
        }

        public void OnAddQuestionTest()
        {
            if (CanAddQuestion())
            {
                _questionRepo.Add(POCO);
            }
        }

        public bool CanAddQuestion()
        {
            if (string.IsNullOrEmpty(Text) || !Answers.AsQueryable().Any(r => r.IsCorrect) || !Answers.AsQueryable().Any(r => !r.IsCorrect))
            {
                return false;
            }

            if(_questionRepo.GetAllItems().AsQueryable().Any(r => r.Text.ToLower().Equals(Text.ToLower())) || Answers.Count() > 4 || Answers.Count() < 2) 
            {
                return false;
            }
            return true;
        }
    }
}
