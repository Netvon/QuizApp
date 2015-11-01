using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Model
{
    public class QuizContext : DbContext
    {
        public QuizContext() : base("name=QuizDB")
        {
            //Database.SetInitializer(new QuizContextInitializer());
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizes { get; set; }
    }
}
