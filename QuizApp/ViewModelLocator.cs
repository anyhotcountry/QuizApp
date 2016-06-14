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
#if DEBUG
                var isPreview = true;
#else
                var isPreview = false;
#endif
                return new QuizPageViewModel(new QuestionsService(), new MediaService(), quizController.Value, isPreview);
            }
        }

        public QuizLauncherPageViewModel QuizLauncherPageViewModel
        {
            get
            {
                var quizPageViewModel = new QuizPageViewModel(new QuestionsService(), new MediaService(), quizController.Value, true);
                return new QuizLauncherPageViewModel(quizController.Value, new PresentationService(quizController.Value, typeof(QuizPage)), quizPageViewModel);
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