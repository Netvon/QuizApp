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
            if (string.IsNullOrWhiteSpace(Name) || _categoryRepo.AsQueryable().Any(c => c.Name == Name))
                return false;

            return true;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var cat = obj as CategoryViewModel;
            return cat.Name == Name;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
