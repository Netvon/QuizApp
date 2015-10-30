using QuizApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizApp.View
{
    /// <summary>
    /// Interaction logic for CategoryEditor.xaml
    /// </summary>
    public partial class CategoryEditor : UserControl
    {
        public ObservableCollection<CategoryViewModel> CategorySource
        {
            get { return (ObservableCollection<CategoryViewModel>)GetValue(CategorySourceProperty); }
            set { SetValue(CategorySourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CategorySource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategorySourceProperty =
            DependencyProperty.Register("CategorySource", 
                typeof(ObservableCollection<CategoryViewModel>), 
                typeof(CategoryEditor),
                new PropertyMetadata(new PropertyChangedCallback(OnCategorySourceChanged)));



        public CategoryViewModel Category
        {
            get { return (CategoryViewModel)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Category.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", 
                typeof(CategoryViewModel), 
                typeof(CategoryEditor), 
                new PropertyMetadata(new PropertyChangedCallback(OnCategoryChanged)));

        static void OnCategoryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as CategoryEditor;
            me.Category = e.NewValue as CategoryViewModel;
        }

        static void OnCategorySourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as CategoryEditor;
            me.CategorySource = e.NewValue as ObservableCollection<CategoryViewModel>;
        }

        public CategoryEditor()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
