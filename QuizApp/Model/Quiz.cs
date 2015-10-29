using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Model
{
    [Table("Quizes")]
    public class Quiz
    {
        [Key]
        public string QuizName { get; set; }
        
        public virtual List<QuizQuestion> Questions { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var q = obj as Quiz;

            return q.QuizName == QuizName;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return QuizName.GetHashCode();
        }
    }
}
