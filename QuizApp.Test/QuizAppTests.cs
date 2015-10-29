using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QuizApp.Model;
using QuizApp.ViewModel;
using System.Collections.Generic;
using System.Linq;
using QuizApp.ViewModel;

namespace QuizApp.Test
{
    [TestClass]
    public class QuizAppTests
    {
        [TestMethod]
        [TestCategory("IRepository")]
        public void IRepository_GetAllItemsReturnAllItemsFromRepo()
        {
            var mock = new Mock<IRepository<Category>>();

            var categories = new List<Category>()
            {
                new Category() { Name = "Category 1" },
                new Category() { Name = "Category 2" },
                new Category() { Name = "Category 3" },
                new Category() { Name = "Category 4" }
            };

            mock.Setup(repo => repo.GetAllItems()).Returns(categories);
            Assert.AreEqual(categories.Count, mock.Object.GetAllItems().Count());
        }

        [TestMethod]
        [TestCategory("IRepository")]
        public void IRepository_AddItemsAddsItemToRepo()
        {
            var mock = new Mock<IRepository<Category>>();

            var categories = new List<Category>()
            {
                new Category() { Name = "Category 1" },
                new Category() { Name = "Category 2" },
                new Category() { Name = "Category 3" },
                new Category() { Name = "Category 4" }
            };

            mock.Setup(repo => repo.GetAllItems()).Returns(categories);

            var cat = new Category() { Name = "Cat 5" };
            mock.Setup(repo => repo.Add(It.IsAny<Category>())).Callback<Category>(c => categories.Add(c));

            Assert.AreEqual(categories.Count, mock.Object.GetAllItems().Count());
            mock.Object.Add(cat);
            Assert.AreEqual(categories.Count, mock.Object.GetAllItems().Count());
        }

        [TestMethod]
        [TestCategory("IRepository")]
        public void IRepository_RemoveItemRemovesItemFromRepo()
        {
            var mock = new Mock<IRepository<Category>>();
            var cat = new Category() { Name = "Cat 5" };

            var categories = new List<Category>()
            {
                new Category() { Name = "Category 1" },
                new Category() { Name = "Category 2" },
                new Category() { Name = "Category 3" },
                cat
            };

            mock.Setup(repo => repo.GetAllItems()).Returns(categories);
            
            mock.Setup(repo => repo.Remove(It.IsAny<Category>())).Callback<Category>(c => categories.Remove(c));

            Assert.AreEqual(categories.Count, mock.Object.GetAllItems().Count());
            mock.Object.Remove(cat);
            Assert.AreEqual(categories.Count, mock.Object.GetAllItems().Count());
        }

        [TestMethod]
        [TestCategory("IRepository")]
        public void IRepository_EditeItemEditsItemInRepo()
        {
            var mock = new Mock<IRepository<Category>>();
            var name = "Cat 5";
            var cat = new Category() { Name = name };

            var categories = new List<Category>()
            {
                new Category() { Name = "Category 1" },
                new Category() { Name = "Category 2" },
                new Category() { Name = "Category 3" },
                cat
            };

            mock.Setup(repo => repo.GetAllItems()).Returns(categories);

            mock.Setup(repo => repo.Edit(It.IsAny<Category>(), It.IsAny<Category>()))
                .Callback<Category, Category>((c1, c2) =>
                {
                    categories.First(c => c.Name == c1.Name).Name = c2.Name;
                });

            Assert.AreEqual(name,cat.Name);
            mock.Object.Edit(cat, new Category() { Name = "Cat 6" });
            Assert.AreNotEqual(name, cat.Name);
        }

        [TestMethod]
        [TestCategory("QuizViewModel")]
        public void QuizViewModel_AddQuiz_CannotAddExistingQuiz()
        {
            var quizRepoMock = new Mock<IRepository<Quiz>>();
            var questionRepoMock = new Mock<IRepository<Question>>();
            var categoryRepoMock = new Mock<IRepository<Category>>();

            var cat1 = new Category() { Name = "Cat1" };

            var categories = new[]
            {
                cat1
            };

            var questions = new[]
            {
                new Question() { Text = "Q1",
                    Answers = new List<Answer>
                {
                    new Answer() { AnswerText ="Q1_A1" },
                    new Answer() { AnswerText ="Q1_A2", IsCorrect = true },
                }, Category = cat1 },
                new Question() { Text = "Q2",
                    Answers = new List<Answer>
                {
                    new Answer() { AnswerText ="Q2_A1" },
                    new Answer() { AnswerText ="Q2_A2", IsCorrect = true },
                }, Category = cat1}
            };

            var quizes = new List<Quiz>()
            {
                new Quiz() { QuizName = "Quiz A" }
            };

            categoryRepoMock.Setup(r => r.GetAllItems()).Returns(categories);
            questionRepoMock.Setup(r => r.GetAllItems()).Returns(questions);
            quizRepoMock.Setup(r => r.GetAllItems()).Returns(quizes);
            quizRepoMock.Setup(r => r.AsQueryable()).Returns(quizes.AsQueryable());

            quizRepoMock.Setup(r => r.Add(It.IsAny<Quiz>())).Callback<Quiz>(q => quizes.Add(q));


            var quiz = new Quiz() { QuizName = "Quiz A" };
            var qqL = new List<QuizQuestion>()
            {
                new QuizQuestion() { Quiz = quiz, Question = questions[0] },
                new QuizQuestion() { Quiz = quiz, Question = questions[1] }
            };
            quiz.Questions = qqL;

            var qvm = new QuizViewModel(quiz, quizRepoMock.Object, questionRepoMock.Object, categoryRepoMock.Object);


            Assert.IsFalse(qvm.AddQuizCommand.CanExecute(null));
        }

        [TestMethod]
        [TestCategory("QuizViewModel")]
        public void QuizViewModel_AddQuiz_CanAddNewQuiz()
        {
            var quizRepoMock = new Mock<IRepository<Quiz>>();
            var questionRepoMock = new Mock<IRepository<Question>>();
            var categoryRepoMock = new Mock<IRepository<Category>>();

            var cat1 = new Category() { Name = "Cat1" };

            var categories = new[]
            {
                cat1
            };

            var questions = new[]
            {
                new Question() { Text = "Q1",
                    Answers = new List<Answer>
                {
                    new Answer() { AnswerText ="Q1_A1" },
                    new Answer() { AnswerText ="Q1_A2", IsCorrect = true },
                }, Category = cat1 },
                new Question() { Text = "Q2",
                    Answers = new List<Answer>
                {
                    new Answer() { AnswerText ="Q2_A1" },
                    new Answer() { AnswerText ="Q2_A2", IsCorrect = true },
                }, Category = cat1}
            };

            var quizes = new List<Quiz>()
            {
                new Quiz() { QuizName = "Quiz A" }
            };

            categoryRepoMock.Setup(r => r.GetAllItems()).Returns(categories);
            questionRepoMock.Setup(r => r.GetAllItems()).Returns(questions);
            quizRepoMock.Setup(r => r.GetAllItems()).Returns(quizes);
            quizRepoMock.Setup(r => r.AsQueryable()).Returns(quizes.AsQueryable());

            quizRepoMock.Setup(r => r.Add(It.IsAny<Quiz>())).Callback<Quiz>(q => quizes.Add(q));


            var quiz = new Quiz() { QuizName = "Quiz B" };
            var qqL = new List<QuizQuestion>()
            {
                new QuizQuestion() { Quiz = quiz, Question = questions[0] },
                new QuizQuestion() { Quiz = quiz, Question = questions[1] }
            };
            quiz.Questions = qqL;

            var qvm = new QuizViewModel(quiz, quizRepoMock.Object, questionRepoMock.Object, categoryRepoMock.Object);


            Assert.IsTrue(qvm.AddQuizCommand.CanExecute(null));
            qvm.AddQuizCommand.Execute(null);
            Assert.AreEqual(2, quizes.Count);
        }

        [TestMethod]
        [TestCategory("CategoryViewModel")]
        public void CategoryViewModel_AddCategory_CannotAddExisting()
        {
            var categories = new List<Category>()
            {
                new Category() { Name = "Category 1" },
                new Category() { Name = "Category 2" },
                new Category() { Name = "Category 3" },
                
            };

            var cat = new Category() { Name = "Category 1" };
            var mock = new Mock<IRepository<Category>>();

            mock.Setup(r => r.GetAllItems()).Returns(categories);
            mock.Setup(repo => repo.Add(It.IsAny<Category>())).Callback<Category>(c => categories.Add(c));

            var catvm = new CategoryViewModel(cat, mock.Object);

            Assert.AreEqual(3, mock.Object.GetAllItems().Count());
           

        }
    }
}
