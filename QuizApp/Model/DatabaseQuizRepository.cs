using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuizApp.Model
{
    public class DatabaseQuizRepository : IRepository<Quiz>
    {
        QuizContext _database;

        public DatabaseQuizRepository(QuizContext databaseContext)
        {
            _database = databaseContext;
        }

        public void Add(Quiz item)
        {
            _database.Quizes.Add(item);
        }

        public void Edit(Quiz oldItem, Quiz newItem)
        {
            var quiz = _database.Quizes.FirstOrDefault(q => q.QuizName == oldItem.QuizName);

            if (quiz == null)
                return;

            quiz.Questions = newItem.Questions;
            quiz.QuizName = newItem.QuizName;
        }

        public IEnumerable<Quiz> GetAllItems()
        {
            return _database.Quizes;
        }

        public void Remove(Quiz item)
        {
            _database.Quizes.Remove(item);
        }

        public void Save()
        {
            _database.SaveChanges();
        }

        public async Task SaveAsync()
        {
//#if DEBUG
//            await Task.Delay(4000);
//#endif
            await _database.SaveChangesAsync();
        }

        public IQueryable<Quiz> AsQueryable()
        {
            return _database.Quizes.AsQueryable();
        }

        public void AddAnswer(Answer answer)
        {

        }
    }
}
