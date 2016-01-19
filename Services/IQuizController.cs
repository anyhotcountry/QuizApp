using System;

namespace QuizApp.Services
{
    public interface IQuizController
    {
        event EventHandler Stop;

        event EventHandler Resume;

        event EventHandler SpeedUp;

        void StopQuiz();

        void ResumeQuiz();

        void SpeedUpQuiz();
    }
}