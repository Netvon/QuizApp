using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using QuizApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.ViewModel
{
    public class GameViewModel : ViewModelBase
    {
        QuizViewModel _quiz;
        QuestionViewModel _currentQuestion;

        public RelayCommand<Answer> SelectAnswerCommand { get; set; }

        public QuizViewModel Quiz
        {
            get
            {
                return _quiz;
            }

            set
            {
                if (_quiz == value)
                {
                    return;
                }

                _quiz = value;
                RaisePropertyChanged();
            }
        }        

        public QuestionViewModel CurrentQuestion
        {
            get
            {
                return _currentQuestion;
            }

            set
            {
                if (_currentQuestion == value)
                {
                    return;
                }

                _currentQuestion = value;
                RaisePropertyChanged();
            }
        }

        public GameViewModel()
        {
            SelectAnswerCommand = new RelayCommand<Answer>(OnSelectAnswer);
        }

        void OnSelectAnswer(Answer obj)
        {
            
        }
    }
}
