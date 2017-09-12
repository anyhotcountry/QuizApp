using Newtonsoft.Json;
using QuizApp.Services;
using QuizApp.Services.ImageSearch;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace QuizApp.ViewModels
{
    public class QuizSetupPageViewModel : Mvvm.ViewModelBase
    {
        private readonly IImageSearchService imageSearchService;
        private readonly IQuestionsService questionsService;
        private string questions;
        private bool busy;

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

        public bool Busy
        {
            get { return busy; }
            set { Set(ref busy, value); }
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
            var previousWords = ImageResults.Select(r => r.Query).ToList();
            var queries = questions.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Where(w => !w.Contains(">")).Select(w => char.ToUpper(w[0]) + w.Substring(1)).ToList();
            var newQueries = queries.Where(w => !previousWords.Contains(w));
            var resultsToDelete = ImageResults.Where(r => !queries.Contains(r.Query)).ToList();
            foreach (var result in resultsToDelete)
            {
                ImageResults.Remove(result);
            }

            foreach (var query in newQueries)
            {
                var optionsPos = query.IndexOf('{');
                var imageQuery = query;
                var searchOptions = new SearchOptions();
                if (optionsPos > 0)
                {
                    try
                    {
                        searchOptions = JsonConvert.DeserializeObject<SearchOptions>(query.Substring(optionsPos));
                        imageQuery = query.Substring(0, optionsPos).Trim();
                    }
                    catch (Exception)
                    {
                        // Do nothing
                    }
                }
                var sources = (await imageSearchService.Search(imageQuery, searchOptions.Animated)).ToList();
                await Task.Delay(200);

                if (sources.Any())
                {
                    var imageResultsViewModel = new ImageResultsViewModel { Query = query, Name = searchOptions.Answer ?? imageQuery };
                    ImageResults.Add(imageResultsViewModel);
                    foreach (var source in sources)
                    {
                        imageResultsViewModel.Images.Add(new ImageViewModel { Uri = source });
                    }
                }
            }
        }

        public async Task Generate()
        {
            Busy = true;
            var otherQuestions = questions.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries).Where(w => w.Contains(">")).Select(w => w.Split('>')).ToDictionary(x => x[1].Trim(), x => x[0].Trim());
            await questionsService.SaveQuiz(ImageResults.ToDictionary(x => x.Name, x => x.SelectedItem.Uri), otherQuestions);
            Busy = false;
        }
    }
}