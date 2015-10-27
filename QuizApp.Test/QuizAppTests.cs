using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QuizApp.Model;
using QuizApp.ViewModel;
using System.Collections.Generic;
using System.Linq;

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
        [TestCategory("CategoryViewModel")]
        //[ExpectedException(typeof(InvalidOperationException))]
        public void CategoryViewModel_AddItemAlreadyExists()
        {
            //var mock = new Mock<IRepository<Category>>();

            //var category = new Category() { Name = "Category 1" };

            //var categoryViewModel = new CategoryViewModel(category, mock.Object);

            //categoryViewModel.OnAddCategoryTest();
            //categoryViewModel.OnAddCategoryTest();

            //Assert.AreEqual(1, mock.Object.GetAllItems().Count());
            ////nog te schrijven
            ////1.toevoegen die bestaat kan niet
            ////2. edit geeft exceptie
            ////3. remove geeft exceptie
        }
    }
}
