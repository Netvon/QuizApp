using GalaSoft.MvvmLight.Threading;
using QuizApp.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace QuizApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherHelper.Initialize();
            //QuizContext context = new QuizContext();
            //Category cat1 = new Category { Name = "Category 1" };
            ////context.Categories.Add(cat1);

            //Question q1 = new Question() { Category = cat1, Text = "Vraag 1" };
            //Question q2 = new Question() { Category = cat1, Text = "Vraag 2" };
            //Question q3 = new Question() { Category = cat1, Text = "Vraag 3" };
            ////context.Questions.Add(q1);
            ////context.Questions.Add(q2);
            ////context.Questions.Add(q3);

            //Answer q1_a1 = new Answer() { AnswerToQuestion = q1, AnswerText = "Antwoord 1", IsCorrect = true };
            //Answer q1_a2 = new Answer() { AnswerToQuestion = q1, AnswerText = "Antwoord 2", IsCorrect = false };
            //Answer q1_a3 = new Answer() { AnswerToQuestion = q1, AnswerText = "Antwoord 3", IsCorrect = false };

            //q1.Answers = new List<Answer>();
            //q1.Answers.Add(q1_a1);
            //q1.Answers.Add(q1_a2);
            //q1.Answers.Add(q1_a3);
            ////context.Answers.Add(q1_a1);
            ////context.Answers.Add(q1_a2);
            ////context.Answers.Add(q1_a3);

            //Answer q2_a1 = new Answer() { AnswerToQuestion = q2, AnswerText = "Antwoord 1", IsCorrect = true };
            //Answer q2_a2 = new Answer() { AnswerToQuestion = q2, AnswerText = "Antwoord 2", IsCorrect = false };
            //Answer q2_a3 = new Answer() { AnswerToQuestion = q2, AnswerText = "Antwoord 3", IsCorrect = false };

            //q2.Answers = new List<Answer>();
            //q2.Answers.Add(q2_a1);
            //q2.Answers.Add(q2_a2);
            //q2.Answers.Add(q2_a3);
            ////context.Answers.Add(q2_a1);
            ////context.Answers.Add(q2_a2);
            ////context.Answers.Add(q2_a3);

            //Answer q3_a1 = new Answer() { AnswerToQuestion = q3, AnswerText = "Antwoord 1", IsCorrect = true };
            //Answer q3_a2 = new Answer() { AnswerToQuestion = q3, AnswerText = "Antwoord 2", IsCorrect = false };
            //Answer q3_a3 = new Answer() { AnswerToQuestion = q3, AnswerText = "Antwoord 3", IsCorrect = false };

            //q3.Answers = new List<Answer>();
            //q3.Answers.Add(q3_a1);
            //q3.Answers.Add(q3_a2);
            //q3.Answers.Add(q3_a3);
            ////context.Answers.Add(q3_a1);
            ////context.Answers.Add(q3_a2);
            ////context.Answers.Add(q3_a3);

            //Quiz quiz1 = new Quiz() { QuizName = "Quiz 1" };
            //quiz1.Questions = new List<QuizQuestion>();

            //QuizQuestion qa_quiz1_q1 = new QuizQuestion() { Quiz = quiz1, Question = q1, GivenAnswer = q1_a2 };
            //quiz1.Questions.Add(qa_quiz1_q1);
            ////context.QuizAnswers.Add(qa_quiz1_q1);

            //QuizQuestion qa_quiz1_q2 = new QuizQuestion() { Quiz = quiz1, Question = q2, GivenAnswer = q2_a1 };
            //quiz1.Questions.Add(qa_quiz1_q2);
            ////context.QuizAnswers.Add(qa_quiz1_q2);

            //QuizQuestion qa_quiz1_q3 = new QuizQuestion() { Quiz = quiz1, Question = q3, GivenAnswer = q3_a1 };
            //quiz1.Questions.Add(qa_quiz1_q3);
            ////context.QuizAnswers.Add(qa_quiz1_q3);

            //context.Quizes.Add(quiz1);

            //context.SaveChanges();
        }
    }
}
