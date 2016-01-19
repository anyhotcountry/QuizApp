using QuizApp.Services;
using QuizApp.ViewModels;
using QuizApp.Views;
using System;

namespace QuizApp
{
    public class ViewModelLocator
    {
        private static readonly Lazy<IQuizController> quizController = new Lazy<IQuizController>(() =>
        {
            return new QuizController();
        });

        public QuizPageViewModel QuizPageViewModel
        {
            get
            {
                return new QuizPageViewModel(QuestionsService.Instance, new MediaService(), quizController.Value);
            }
        }

        public QuizLauncherPageViewModel QuizLauncherPageViewModel
        {
            get
            {
                return new QuizLauncherPageViewModel(quizController.Value, new PresentationService(typeof(QuizPage)));
            }
        }


        public AnswersPageViewModel AnswersPageViewModel
        {
            get { return new AnswersPageViewModel(QuestionsService.Instance); }
        }
    }
}
