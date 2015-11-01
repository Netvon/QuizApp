using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Model
{
    public interface IRepository<T>
    {
        void Add(T item);
        void Remove(T item);
        void Edit(T oldItem, T newItem);
        Task SaveAsync();
        void Save();
        IEnumerable<T> GetAllItems();
        IQueryable<T> AsQueryable();

        void AddAnswer(Answer Answer);
    }
}
