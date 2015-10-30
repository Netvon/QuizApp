using GalaSoft.MvvmLight.Command;
using QuizApp.Helpers;
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
                AddCategoryCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddCategoryCommand { get; set; }

        readonly INotificationService _notificationService;

        public CategoryViewModel(Category poco, IRepository<Category> categoryRepo, INotificationService notificationService)
            : base(poco)
        {
            _notificationService = notificationService;
            _categoryRepo = categoryRepo;

            AddCategoryCommand = new RelayCommand(OnAddCategory, CanAddCategory);
        }

        async void OnAddCategory()
        {
            _categoryRepo.Add(POCO);

            _notificationService.StartLoading("CategoryViewModel");
            await _categoryRepo.SaveAsync();
            _notificationService.StopLoading("CategoryViewModel");
            _notificationService.SendMessage("CategoryViewModel", "CatAdded");

            POCO = new Category();
            RaisePropertyChanged("Name");
            AddCategoryCommand.RaiseCanExecuteChanged();
        }

        //test
        public void OnAddCategoryTest()
        {
            if (CanAddCategory())
            {
                _categoryRepo.Add(POCO);
            }
           
        }

        bool CanAddCategory()
        {
            if (string.IsNullOrEmpty(Name) || _categoryRepo.AsQueryable().Any(c => c.Name == Name))
                return false;

            return true;
        }
    }
}
