using System;

namespace QuizApp.Services
{
    public interface IQuizController
    {
        event EventHandler Stop;

        event EventHandler Resume;

        event EventHandler SpeedUp;

        event EventHandler NextQuestion;

        void StopQuiz();

        void ResumeQuiz();

        void SpeedUpQuiz();

        void EndQuestion();
    }
}