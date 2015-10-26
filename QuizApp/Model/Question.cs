using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Model
{
    [Table("Questions")]
    public class Question
    {
        [Key]
        public int QuestionID { get; set; }

        public string CategoryName { get; set; }

        [ForeignKey("CategoryName")]
        public Category Category { get; set; }

        public virtual List<Answer> Answers { get; set; }

        public virtual List<Quiz> Quizes { get; set; }

        public string Text { get; set; }
    }
}
