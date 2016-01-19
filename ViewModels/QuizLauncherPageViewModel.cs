using QuizApp.Services;
using System.Collections.Generic;
using Template10.Mvvm;

namespace QuizApp.ViewModels
{
    public class QuizLauncherPageViewModel : Mvvm.ViewModelBase
    {
        private readonly IQuestionsService questionsService;
        private readonly List<string> questions;
        private readonly IMediaService mediaService;
        private readonly IPresentationService presentationService;
        private readonly IQuizController quizController;
        private static int questionIndex;
        private IQuestionViewModel currentViewModel;
        private static bool stopped = true;

        public QuizLauncherPageViewModel(IQuizController quizController, IPresentationService presentationService)
        {
            this.quizController = quizController;
            this.presentationService = presentationService;
            LaunchCommand = new DelegateCommand(LaunchExecute, LaunchCanExecute);
            PauseCommand = new DelegateCommand(PauseExecute, PauseCanExecute);
            ResumeCommand = new DelegateCommand(ResumeExecute, ResumeCanExecute);
        }

        public DelegateCommand LaunchCommand { get; }

        public DelegateCommand PauseCommand { get; }

        public DelegateCommand ResumeCommand { get; }

        private bool LaunchCanExecute()
        {
            return stopped;
        }

        private async void LaunchExecute()
        {
            await presentationService.ProjectAsync().ConfigureAwait(false);
        }

        private bool PauseCanExecute()
        {
            return true;
        }

        private async void PauseExecute()
        {
            quizController.StopQuiz();
        }

        private bool ResumeCanExecute()
        {
            return true;
        }

        private async void ResumeExecute()
        {
            quizController.ResumeQuiz();
        }
    }
}