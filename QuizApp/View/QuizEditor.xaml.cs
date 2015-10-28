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
    /// Interaction logic for QuizEditor.xaml
    /// </summary>
    public partial class QuizEditor : UserControl
    {

        public ObservableCollection<QuestionViewModel> QuestionSource
        {
            get { return (ObservableCollection<QuestionViewModel>)GetValue(QuestionSourceProperty); }
            set { SetValue(QuestionSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for QuestionSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QuestionSourceProperty =
            DependencyProperty.Register("QuestionSource", 
                typeof(ObservableCollection<QuestionViewModel>), 
                typeof(QuizEditor), 
                new PropertyMetadata(new ObservableCollection<QuestionViewModel>(), 
                                     new PropertyChangedCallback(OnQuestionSourceChanged)));


        public QuizViewModel Quiz
        {
            get { return (QuizViewModel)GetValue(QuizProperty); }
            set { SetValue(QuizProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Quiz.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QuizProperty =
            DependencyProperty.Register("Quiz", 
                typeof(QuizViewModel), 
                typeof(QuizEditor), 
                new PropertyMetadata(new PropertyChangedCallback(OnQuizChanged)));

        static void OnQuizChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as QuizEditor;
            me.Quiz = e.NewValue as QuizViewModel;
        }

        static void OnQuestionSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as QuizEditor;
            me.QuestionSource = e.NewValue as ObservableCollection<QuestionViewModel>;
        }

        public QuizEditor()
        {
            InitializeComponent();
        }
    }
}
