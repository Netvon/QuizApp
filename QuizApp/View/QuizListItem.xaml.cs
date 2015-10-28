﻿using QuizApp.ViewModel;
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
    /// Interaction logic for QuizListItem.xaml
    /// </summary>
    public partial class QuizListItem : UserControl
    {

        public QuizViewModel Quiz
        {
            get { return (QuizViewModel)GetValue(QuizProperty); }
            set { SetValue(QuizProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Quiz.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QuizProperty =
            DependencyProperty.Register("Quiz", typeof(QuizViewModel), typeof(QuizListItem), new PropertyMetadata());


        public QuizListItem()
        {
            //DataContext = Quiz;

            InitializeComponent();            
        }
    }
}