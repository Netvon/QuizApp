using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Model
{
    public class DatabaseQuestionRepository : IRepository<Question>
    {
        QuizContext _database;

        public DatabaseQuestionRepository(QuizContext databaseContext)
        {
            _database = databaseContext;
        }

        public void Add(Question item)
        {
            _database.Questions.Add(item);
        }

        public void AddAnswer(Answer item)
        {
            _database.Answers.Add(item);
        }

        public void Edit(Question oldItem, Question newItem)
        {
            var cat = _database.Questions.FirstOrDefault(c => c.QuestionID == oldItem.QuestionID);

            if (cat == null)
                return;

            cat.Category = newItem.Category;
            cat.Text = newItem.Text;
            cat.Answers = newItem.Answers;
        }

        public IEnumerable<Question> GetAllItems()
        {
            return _database.Questions;
        }

        public void Remove(Question item)
        {
            _database.Questions.Remove(item);
        }

        public void Save()
        {
            _database.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _database.SaveChangesAsync();
        }

        public IQueryable<Question> AsQueryable()
        {
            return _database.Questions.AsQueryable();
        }
    }
}