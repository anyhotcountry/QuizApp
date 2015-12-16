using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QuizApp.ViewModels
{
    public class QuizPageViewModel : Mvvm.ViewModelBase
    {
        private readonly List<string> questions;
        private readonly MediaElement mediaElement;
        private int questionIndex;
        private IQuestionViewModel currentViewModel;

        public QuizPageViewModel()
        {
            questions = new List<string>();
            mediaElement = new MediaElement();
            Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => LoadQuestions());
        }

        public MediaElement SoundPlayer
        {
            get { return mediaElement; }
        }

        public IQuestionViewModel CurrentViewModel
        {
            get { return currentViewModel; }

            private set { Set(ref currentViewModel, value); }
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
                    CurrentViewModel = new TriviaQuestionViewModel(filename, ++questionIndex, mediaElement);
                    CurrentViewModel.QuestionFinished += CurrentViewModelOnQuestionFinished;
                }

                if (extension == ".jpg" || extension == ".png")
                {
                    CurrentViewModel = new PhotoQuestionViewModel(filename, ++questionIndex, mediaElement);
                    CurrentViewModel.QuestionFinished += CurrentViewModelOnQuestionFinished;
                }
            }
            else
            {
                CurrentViewModel = null;
            }
        }

        private void CurrentViewModelOnQuestionFinished(object sender, EventArgs e)
        {
            ChangeViewModel();
        }
    }
}