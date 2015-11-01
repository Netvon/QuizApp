using System;
using System.Collections.Generic;
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
    /// Interaction logic for QuestionListItem.xaml
    /// </summary>
    public partial class QuestionListItem : UserControl
    {


        public Visibility DetailsVisibility
        {
            get { return (Visibility)GetValue(DetailsVisibilityProperty); }
            set { SetValue(DetailsVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DetailsVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DetailsVisibilityProperty =
            DependencyProperty.Register("DetailsVisibility", typeof(Visibility), typeof(QuestionListItem), new PropertyMetadata(Visibility.Visible));


        public QuestionListItem()
        {
            InitializeComponent();
        }
    }
}
