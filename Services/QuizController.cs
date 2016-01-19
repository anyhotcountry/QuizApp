using System;

namespace QuizApp.Services
{
    public class QuizController : IQuizController
    {
        public event EventHandler Stop;

        public event EventHandler Resume;

        public event EventHandler SpeedUp;

        public void StopQuiz()
        {
            Stop?.Invoke(this, EventArgs.Empty);
        }

        public void ResumeQuiz()
        {
            Resume?.Invoke(this, EventArgs.Empty);
        }

        public void SpeedUpQuiz()
        {
            SpeedUp?.Invoke(this, EventArgs.Empty);
        }
    }
}
