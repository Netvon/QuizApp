using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Model
{
    public class DatabaseCategoryRepository : IRepository<Category>
    {
        QuizContext _database;

        public DatabaseCategoryRepository(QuizContext databaseContext)
        {
            _database = databaseContext;
        }

        public void Add(Category item)
        {
            _database.Categories.Add(item);
        }

        public void Edit(Category oldItem, Category newItem)
        {
            throw new InvalidOperationException("Category can not be edited");
        }

        public IEnumerable<Category> GetAllItems()
        {
            return _database.Categories;
        }

        public void Remove(Category item)
        {
            throw new InvalidOperationException("Category can not be removed");
        }

        public void Save()
        {
            _database.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _database.SaveChangesAsync();
        }

        public IQueryable<Category> AsQueryable()
        {
            return _database.Categories.AsQueryable();
        }

        public void AddAnswer(Answer Answer)
        {
            throw new NotImplementedException();
        }
    }
}
