using QuizApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace QuizApp.ViewModels
{
    public class MainPageViewModel : Mvvm.ViewModelBase
    {
        private readonly IImageSearchService imageSearchService;
        private string questions;

        public MainPageViewModel(IImageSearchService imageSearchService)
        {
            this.imageSearchService = imageSearchService;
        }

        public ObservableCollection<ImageSource> Sources { get; } = new ObservableCollection<ImageSource>();

        public string Questions
        {
            get { return questions; }
            set { Set(ref questions, value); }
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            await Task.Yield();
        }

        public async Task SelectFiles()
        {
            await QuestionsService.Instance.PickFiles();
        }

        public async Task Search()
        {
            if (string.IsNullOrEmpty(questions))
            {
                return;
            }

            var queries = questions.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var query in queries)
            {
                var sources = await imageSearchService.Search(query, 10);
                foreach (var source in sources)
                {
                    Sources.Add(source);
                }
            }
        }
    }
}