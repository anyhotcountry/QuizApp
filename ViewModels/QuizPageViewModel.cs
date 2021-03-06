using QuizApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;

namespace QuizApp.ViewModels
{
    public class QuizPageViewModel : Mvvm.ViewModelBase
    {
        private readonly IQuestionsService questionsService;
        private readonly List<string> questions;
        private readonly IMediaService mediaService;
        private readonly IQuizController quizController;
        private readonly CoreDispatcher dispatcher;
        private readonly bool isPreview;
        private int questionIndex;
        private IQuestionViewModel currentViewModel;
        private bool stopped = true;

        public QuizPageViewModel(IQuestionsService questionsService, IMediaService mediaService, IQuizController quizController, bool isPreview)
        {
            this.questionsService = questionsService;
            this.isPreview = isPreview;
            questions = new List<string>();
            this.mediaService = mediaService;
            this.quizController = quizController;
            quizController.Stop += QuizControllerOnStop;
            quizController.Resume += QuizControllerOnResume;
            quizController.NextQuestion += QuizControllerOnNextQuestion;
            dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
        }

        private async void QuizControllerOnNextQuestion(object sender, EventArgs e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => CurrentViewModel?.End());
        }

        private async void QuizControllerOnResume(object sender, EventArgs e)
        {
            stopped = false;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => CurrentViewModel?.Start());
        }

        private async void QuizControllerOnStop(object sender, EventArgs e)
        {
            stopped = true;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => CurrentViewModel?.Stop());
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

        public void OnUnLoaded()
        {
            CurrentViewModel?.Stop();
            stopped = true;
        }

        public async Task Start()
        {
            questionIndex = 0;
            stopped = false;
            await LoadQuestions().ConfigureAwait(false);
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            OnUnLoaded();
            return base.OnNavigatedFromAsync(state, suspending);
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
                if (extension == ".trivia")
                {
                    CurrentViewModel = new TriviaQuestionViewModel(filename, ++questionIndex, questionsService, mediaService, isPreview);
                    CurrentViewModel.QuestionFinished += CurrentViewModelOnQuestionFinished;
                }
                else if (extension == ".jumble")
                {
                    CurrentViewModel = new JumbleQuestionViewModel(filename, ++questionIndex, questionsService, mediaService, isPreview);
                    CurrentViewModel.QuestionFinished += CurrentViewModelOnQuestionFinished;
                }
                else if (filename.Contains(".trivia.") && (extension == ".jpg" || extension == ".png"))
                {
                    CurrentViewModel = new TriviaPhotoQuestionViewModel(filename, ++questionIndex, questionsService, mediaService, isPreview);
                    CurrentViewModel.QuestionFinished += CurrentViewModelOnQuestionFinished;
                }
                else if (extension == ".jpg" || extension == ".png")
                {
                    CurrentViewModel = new PhotoQuestionViewModel(filename, ++questionIndex, questionsService, mediaService, isPreview);
                    CurrentViewModel.QuestionFinished += CurrentViewModelOnQuestionFinished;
                }
                else if (extension == ".action")
                {
                    CurrentViewModel = new ActionChallengeViewModel(filename, ++questionIndex, questionsService, mediaService, isPreview);
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
            if (!stopped)
            {
                ChangeViewModel();
            }
        }
    }
}