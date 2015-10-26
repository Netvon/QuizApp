using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Model
{
    public class DesignTimeCategoryRepository : IRepository<Category>
    {
        public void Add(Category item)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Category> AsQueryable()
        {
            return new[]
            {
                new Category() { Name = "Category 1" },
                new Category() { Name = "Category 2" }
            }.AsQueryable();
        }

        public void Edit(Category oldItem, Category newItem)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetAllItems()
        {
            return new[]
            {
                new Category() { Name = "Category 1" },
                new Category() { Name = "Category 2" }
            };
        }

        public void Remove(Category item)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
