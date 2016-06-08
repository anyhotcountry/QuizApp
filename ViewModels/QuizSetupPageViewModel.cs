using QuizApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace QuizApp.ViewModels
{
    public class QuizSetupPageViewModel : Mvvm.ViewModelBase
    {
        private readonly IImageSearchService imageSearchService;
        private readonly IQuestionsService questionsService;
        private string questions;

        public QuizSetupPageViewModel(IQuestionsService questionsService, IImageSearchService imageSearchService)
        {
            this.questionsService = questionsService;
            this.imageSearchService = imageSearchService;
        }

        public string Questions
        {
            get { return questions; }
            set { Set(ref questions, value); }
        }

        public ObservableCollection<ImageResultsViewModel> ImageResults { get; } = new ObservableCollection<ImageResultsViewModel>();

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            await Task.Yield();
        }

        public async Task SelectFiles()
        {
            await questionsService.PickFiles();
        }

        public async Task Search()
        {
            if (string.IsNullOrEmpty(questions))
            {
                return;
            }

            var previousWords = ImageResults.Select(r => r.Name).ToList();
            var queries = questions.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(w => char.ToUpper(w[0]) + w.Substring(1))
                .Where(w => !previousWords.Contains(w));
            foreach (var query in queries)
            {
                var imageResultsViewModel = new ImageResultsViewModel { Name = query };
                ImageResults.Add(imageResultsViewModel);
                var sources = await imageSearchService.Search(query, 20);
                foreach (var source in sources)
                {
                    imageResultsViewModel.Images.Add(new ImageViewModel { Uri = source });
                }
            }
        }

        public async Task Generate()
        {
            questionsService.SaveQuiz(ImageResults.ToDictionary(x => x.Name, x => x.SelectedItem as BitmapImage));
        }
    }
}