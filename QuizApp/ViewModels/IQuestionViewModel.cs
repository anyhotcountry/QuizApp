using System;

namespace QuizApp.ViewModels
{
    public interface IQuestionViewModel
    {
        event EventHandler QuestionFinished;

        void Stop();
    }
}