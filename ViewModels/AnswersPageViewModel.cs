using QuizApp.Services;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace QuizApp.ViewModels
{
    public class AnswersPageViewModel : Mvvm.ViewModelBase
    {
        private string answers;
        private readonly IPrintService printService;
        private readonly IQuestionsService questionsService;

        public AnswersPageViewModel(IQuestionsService questionsService, IPrintService printService)
        {
            this.questionsService = questionsService;
            this.printService = printService;
            Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                Answers = await questionsService.GetAnswersAsync();
            });
        }

        public string Answers
        {
            get { return answers; }

            private set { Set(ref answers, value); }
        }

        public async Task Print()
        {
            await printService.ShowPrintUIAsync();
        }
    }
}