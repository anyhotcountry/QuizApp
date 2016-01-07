using QuizApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Storage;

namespace QuizApp.ViewModels
{
    public class QuizPageViewModel : Mvvm.ViewModelBase
    {
        private readonly IQuestionsService questionsService;
        private readonly List<string> questions;
        private readonly IMediaService mediaService;
        private readonly IPresentationService presentationService;
        private int questionIndex;
        private IQuestionViewModel currentViewModel;
        private bool stopped = true;

        public QuizPageViewModel(IQuestionsService questionsService, IMediaService mediaService, IPresentationService presentationService)
        {
            this.questionsService = questionsService;
            questions = new List<string>();
            this.mediaService = mediaService;
            this.presentationService = presentationService;
            LaunchCommand = new DelegateCommand(LaunchExecute, LaunchCanExecute);
            PauseCommand = new DelegateCommand(PauseExecute, PauseCanExecute);
            ResumeCommand = new DelegateCommand(ResumeExecute, ResumeCanExecute);
        }

        public object SoundPlayer
        {
            get { return mediaService.MediaElement; }
        }

        public IQuestionViewModel CurrentViewModel
        {
            get { return currentViewModel; }

            private set { Set(ref currentViewModel, value); }
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
            CurrentViewModel?.Stop();
        }

        private bool ResumeCanExecute()
        {
            return true;
        }

        private async void ResumeExecute()
        {
            CurrentViewModel?.Start();
        }

        public void OnUnLoaded()
        {
            CurrentViewModel?.Stop();
            stopped = true;
        }

        public async Task OnLoaded()
        {
            stopped = false;
            await LoadQuestions().ConfigureAwait(false);
        }

        private async Task LoadQuestions()
        {
            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Quiz");
            if (folder == null)
            {
                return;
            }

            var files = await folder.GetFilesAsync();
            foreach (var item in files)
            {
                questions.Add(item.Path);
            }

            ChangeViewModel();
        }

        private void ChangeViewModel()
        {
            if (questionIndex < questions.Count)
            {
                var filename = questions[questionIndex];
                var extension = Path.GetExtension(filename);
                if (extension == ".txt")
                {
                    CurrentViewModel = new TriviaQuestionViewModel(filename, ++questionIndex, questionsService, mediaService);
                    CurrentViewModel.QuestionFinished += CurrentViewModelOnQuestionFinished;
                }
                else if (extension == ".jpg" || extension == ".png")
                {
                    CurrentViewModel = new PhotoQuestionViewModel(filename, ++questionIndex, questionsService, mediaService);
                    CurrentViewModel.QuestionFinished += CurrentViewModelOnQuestionFinished;
                }
                else
                {
                    questionIndex++;
                    ChangeViewModel();
                }
            }
            else
            {
                CurrentViewModel = null;
                stopped = true;
            }
        }

        private void CurrentViewModelOnQuestionFinished(object sender, EventArgs e)
        {
            ChangeViewModel();
        }
    }
}