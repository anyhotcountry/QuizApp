using System.Collections.Generic;

namespace QuizApp.Models
{
    public class Quiz
    {
        public IList<IQuestion> Questions { get; set; }
    }
}