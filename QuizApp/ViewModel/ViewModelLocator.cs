/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:QuizApp"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using QuizApp.Helpers;
using QuizApp.Model;

namespace QuizApp.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<IWindowService, WpfWindowService>();
            SimpleIoc.Default.Register<INotificationService, WpfNotificationService>();



            SimpleIoc.Default.Register<QuizContext>();

            if (ViewModelBase.IsInDesignModeStatic)
                SimpleIoc.Default.Register<IRepository<Category>, DesignTimeCategoryRepository>();
            else
                SimpleIoc.Default.Register<IRepository<Category>, DatabaseCategoryRepository>();

            SimpleIoc.Default.Register<IRepository<Question>, DatabaseQuestionRepository>();
            SimpleIoc.Default.Register<IRepository<Quiz>, DatabaseQuizRepository>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<EditorViewModel>();
            //SimpleIoc.Default.Register<GameViewModel>();

        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public EditorViewModel Editor
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditorViewModel>();
            }
        }

        //public GameViewModel Game
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<GameViewModel>();
        //    }
        //}

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}