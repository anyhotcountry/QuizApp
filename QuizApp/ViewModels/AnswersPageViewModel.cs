using QuizApp.Services;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace QuizApp.ViewModels
{
    public class AnswersPageViewModel : Mvvm.ViewModelBase
    {
        private string answers;
        private readonly QuestionsService questionsService;

        public AnswersPageViewModel()
        {
            questionsService = QuestionsService.Instance;
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
    }
}