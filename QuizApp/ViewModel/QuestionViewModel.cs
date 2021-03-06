﻿using QuizApp.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizApp.Helpers;

namespace QuizApp.ViewModel
{
    public class QuestionViewModel : POCOViewModel<Question>
    {
        IRepository<Question> _questionRepo;
        IRepository<Category> _catRepo;

        CategoryViewModel _category;

        public RelayCommand AddQuestionCommand { get; set; }
        public RelayCommand RemoveQuestionCommand { get; set; }

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

        public string CategoryName
        {
            get
            {
                return POCO.CategoryName;
            }
            set
            {
                POCO.CategoryName = value;
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
                CategoryName = _category.Name;
                POCO.Category = _category.POCO;
                RaisePropertyChanged("Category");
            }
        }

        public ObservableCollection<Answer> Answers { get; set; }

        readonly INotificationService _notificationService;

        public QuestionViewModel(Question poco, IRepository<Question> questionRepo, IRepository<Category> categoryRepo, INotificationService notificationService)
            : base(poco)
        {
            _notificationService = notificationService;
            _questionRepo = questionRepo;
            _catRepo = categoryRepo;
            Answers = new ObservableCollection<Answer>(poco.Answers);
            Category = new CategoryViewModel(poco.Category, _catRepo, _notificationService);

            AddQuestionCommand = new RelayCommand(OnAddQuestion, CanAddQuestion);
            RemoveQuestionCommand = new RelayCommand(OnRemoveQuestion, CanRemoveQuestion);
        }

        async void OnAddQuestion()
        {
           
            _questionRepo.Add(POCO);

            _notificationService.StartLoading("QuestionViewModel");
            await _questionRepo.SaveAsync();
            _notificationService.StopLoading("QuestionViewModel");
        }

        async void OnRemoveQuestion()
        {
            _questionRepo.Remove(POCO);

            _notificationService.StartLoading("QuestionViewModel");
            await _questionRepo.SaveAsync();
            _notificationService.StopLoading("QuestionViewModel");
        }

        public bool CanRemoveQuestion()
        {
            return !string.IsNullOrEmpty(POCO.Text);
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
            if (string.IsNullOrEmpty(Text) || !POCO.Answers.AsQueryable().Any(r => r.IsCorrect) || !POCO.Answers.AsQueryable().Any(r => !r.IsCorrect))
            {
                return false;
            }

            if (Answers.Count() > 4 || Answers.Count() < 2 || POCO.Category == null)
            {
                return false;
            }
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
            var question = obj as QuestionViewModel;
            return question.POCO == POCO;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return POCO.GetHashCode();
        }
    }
}
