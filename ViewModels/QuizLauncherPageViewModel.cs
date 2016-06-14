using QuizApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Controls;

namespace QuizApp.ViewModels
{
    public class QuizLauncherPageViewModel : Mvvm.ViewModelBase
    {
        private readonly IPresentationService presentationService;
        private readonly IQuizController quizController;
        private bool hasLaunched = false;
        private bool isPlaying = false;
        private Symbol controlSymbol;
        private string controlText;

        public QuizLauncherPageViewModel(IQuizController quizController, IPresentationService presentationService, QuizPageViewModel quizPageViewModel)
        {
            this.quizController = quizController;
            this.presentationService = presentationService;
            this.QuizPageViewModel = quizPageViewModel;
            ControlCommand = new DelegateCommand(ControlExecute);
            NextCommand = new DelegateCommand(NextExecute);
            ControlText = "Launch Quiz";
            ControlSymbol = Symbol.Play;
        }

        public DelegateCommand ControlCommand { get; }

        public DelegateCommand NextCommand { get; }

        public Symbol ControlSymbol
        {
            get { return controlSymbol; }

            set { Set(ref controlSymbol, value); }
        }

        public string ControlText
        {
            get { return controlText; }

            set { Set(ref controlText, value); }
        }

        public QuizPageViewModel QuizPageViewModel { get; }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            QuizPageViewModel.OnUnLoaded();
            return base.OnNavigatedFromAsync(state, suspending);
        }

        private async void ControlExecute()
        {
            if (!hasLaunched)
            {
                hasLaunched = true;
                ControlSymbol = Symbol.Pause;
                ControlText = "Pause Quiz";
                isPlaying = true;

                await presentationService.ProjectAsync().ConfigureAwait(false);
            }
            else if (isPlaying)
            {
                ControlSymbol = Symbol.Play;
                ControlText = "Resume Quiz";
                quizController.StopQuiz();
                isPlaying = false;
            }
            else
            {
                ControlSymbol = Symbol.Pause;
                ControlText = "Pause Quiz";
                quizController.ResumeQuiz();
                isPlaying = true;
            }
        }

        private async void NextExecute()
        {
            quizController.EndQuestion();
        }
    }
}