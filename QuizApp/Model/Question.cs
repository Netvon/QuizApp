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
        public virtual Category Category { get; set; }

        public virtual List<Answer> Answers { get; set; }

        public virtual List<Quiz> Quizes { get; set; }

        public string Text { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var q = obj as Question;
            return q.QuestionID == QuestionID;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return QuestionID.GetHashCode();
        }
    }
}
