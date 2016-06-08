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
                return new QuizPageViewModel(new QuestionsService(), new MediaService(), quizController.Value);
            }
        }

        public QuizLauncherPageViewModel QuizLauncherPageViewModel
        {
            get
            {
                return new QuizLauncherPageViewModel(quizController.Value, new PresentationService(typeof(QuizPage)));
            }
        }

        public QuizSetupPageViewModel QuizSetupViewModel
        {
            get
            {
                return new QuizSetupPageViewModel(new QuestionsService(), new ImageSearchService());
            }
        }

        public AnswersPageViewModel AnswersPageViewModel
        {
            get { return new AnswersPageViewModel(new QuestionsService(), new PrintService(new PrintAnswersPage())); }
        }

        public PrintAnswersPageViewModel PrintAnswersPageViewModel
        {
            get { return new PrintAnswersPageViewModel(new QuestionsService()); }
        }
    }
}