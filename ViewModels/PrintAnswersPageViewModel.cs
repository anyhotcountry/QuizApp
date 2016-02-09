using QuizApp.Services;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace QuizApp.ViewModels
{
    public class PrintAnswersPageViewModel : Mvvm.ViewModelBase
    {
        private IList<string> answers1;
        private IList<string> answers2;
        private readonly IQuestionsService questionsService;

        public PrintAnswersPageViewModel(IQuestionsService questionsService)
        {
            this.questionsService = questionsService;
            Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                var answers = await questionsService.GetAnswersListAsync();
                answers = answers.Select((x, i) => $"{i + 1}. {x}").ToList();
                Answers1 = new List<string>(answers.Take(answers.Count / 2));
                Answers2 = new List<string>(answers.Skip(answers.Count / 2));
            });
        }

        public IList<string> Answers1
        {
            get { return answers1; }

            set { Set(ref answers1, value); }
        }

        public IList<string> Answers2
        {
            get { return answers2; }

            set { Set(ref answers2, value); }
        }
    }
}