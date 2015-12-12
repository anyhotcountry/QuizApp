using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace QuizApp.ViewModels
{
    public class QuizPageViewModel : Mvvm.ViewModelBase
    {
        private List<string> Questions;
        private readonly DispatcherTimer gameTimer;
        private readonly MediaElement mediaElement;
        private readonly Random random;
        private string answer;
        private BitmapImage imageSource;
        private bool isCollapsed;
        private int questionIndex = 1;
        private double takeCount;

        public QuizPageViewModel()
        {
            random = new Random();
            mediaElement = new MediaElement();
            gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            gameTimer.Tick += (s, e) => GameTimerOnTick();
            gameTimer.Start();
        }

        public string Answer
        {
            get { return answer; }

            set { Set(ref answer, value); }
        }

        public ObservableCollection<BlockViewModel> Blocks { get; } = new ObservableCollection<BlockViewModel>();

        public BitmapImage ImageSource
        {
            get { return imageSource; }

            set { Set(ref imageSource, value); }
        }

        public bool IsCollapsed
        {
            get { return isCollapsed; }

            set { Set(ref isCollapsed, value); }
        }

        public int QuestionIndex
        {
            get { return questionIndex; }

            set { Set(ref questionIndex, value); }
        }

        public MediaElement SoundPlayer
        {
            get { return mediaElement; }
        }

        private async void GameTimerOnTick()
        {
            gameTimer.Stop();
            if (Questions == null)
            {
                var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Quiz");
                if (folder == null)
                {
                    return;
                }

                var files = await folder.GetFilesAsync();
                Questions = new List<string>();
                foreach (var item in files)
                {
                    Questions.Add(item.Path);
                }

                NextQuestion();
            }

            for (int i = 0; i < (int)takeCount; i++)
            {
                if (Blocks.Count != 0)
                {
                    var index = random.Next(Blocks.Count);
                    Blocks.RemoveAt(index);
                }
            }

            if (Blocks.Count == 0)
            {
                var answerRaw = Path.GetFileNameWithoutExtension(Questions[questionIndex - 1]);
                answerRaw = answerRaw.Replace("Zoomed_", string.Empty).Replace("_", " ");
                Answer = Regex.Replace(answerRaw, "(?!^)([A-Z])", " $1").Trim();
                IsCollapsed = false;
                using (var speechSynthesizer = new SpeechSynthesizer())
                {
                    var stream = await speechSynthesizer.SynthesizeTextToStreamAsync(answer);
                    mediaElement.SetSource(stream, stream.ContentType);
                    mediaElement.Play();
                }

                await Task.Delay(TimeSpan.FromSeconds(3));
            }

            if (Blocks.Count == 0 && questionIndex < Questions.Count)
            {
                QuestionIndex++;
                NextQuestion();
                gameTimer.Start();
            }

            if (Blocks.Count > 0)
            {
                takeCount += 0.02;
                gameTimer.Start();
            }
        }

        private async void NextQuestion()
        {
            var width = 4;
            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    Blocks.Add(new BlockViewModel(width * j, width * i, width, width));
                }
            }

            var file = await StorageFile.GetFileFromPathAsync(Questions[questionIndex - 1]);
            var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            var img = new BitmapImage();
            img.SetSource(fileStream);

            ImageSource = img;
            Answer = string.Empty;
            IsCollapsed = true;
#if DEBUG
            takeCount = 50;
#else
            takeCount = 1;
#endif
        }
    }
}