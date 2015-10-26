using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Model
{
    [Table("Answers")]
    public class Answer
    {
        [Key, Column(Order = 2)]
        public string AnswerText { get; set; }

        [Key, ForeignKey("AnswerToQuestion"), Column(Order = 1)]
        public int QuestionID { get; set; }
        
        public Question AnswerToQuestion { get; set; }

        public bool IsCorrect { get; set; }

    }
}
