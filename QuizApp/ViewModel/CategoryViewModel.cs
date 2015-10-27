using GalaSoft.MvvmLight.Command;
using QuizApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.ViewModel
{
    public class CategoryViewModel : POCOViewModel<Category>
    {
        IRepository<Category> _categoryRepo;

        public string Name
        {
            get
            {
                return POCO.Name;
            }
            set
            {
                POCO.Name = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand AddCategoryCommand { get; set; }

        public CategoryViewModel(Category poco, IRepository<Category> categoryRepo)
            : base(poco)
        {
            _categoryRepo = categoryRepo;

            AddCategoryCommand = new RelayCommand(OnAddCategory, CanAddCategory);
        }

        async void OnAddCategory()
        {
            _categoryRepo.Add(POCO);

            await _categoryRepo.SaveAsync();
        }

        //test
        public void OnAddCategoryTest()
        {
            _categoryRepo.Add(POCO);
        }

        bool CanAddCategory()
        {
            if (string.IsNullOrEmpty(Name) || _categoryRepo.AsQueryable().Any(c => c.Name.Contains(Name)))
                return false;

            return true;
        }
    }
}
