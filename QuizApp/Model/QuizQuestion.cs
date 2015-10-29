using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Model
{
    [Table("QuizQuestions")]
    public class QuizQuestion
    {
        [Key, ForeignKey("Quiz"), Column(Order = 1)]
        public string QuizName { get; set; }

        [Key, ForeignKey("GivenAnswer"), Column(Order = 2)]
        public int QuestionID { get; set; }

        [ForeignKey("GivenAnswer"), Column(Order = 3)]
        public string GivenAnswerText { get; set; }

        public virtual Question Question { get; set; }
        public Quiz Quiz { get; set; }
        public Answer GivenAnswer { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var qq = obj as QuizQuestion;

            return qq.Question == Question && 
                   qq.Quiz == Quiz;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return (Quiz.GetHashCode() + Question.GetHashCode()).GetHashCode();
        }

    }
}
