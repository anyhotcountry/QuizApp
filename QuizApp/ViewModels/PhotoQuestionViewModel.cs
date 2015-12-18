using QuizApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace QuizApp.ViewModels
{
    public class PhotoQuestionViewModel : Mvvm.ViewModelBase, IQuestionViewModel
    {
        private readonly DispatcherTimer gameTimer;
        private readonly MediaElement mediaElement;
        private readonly Random random;
        private readonly string filename;
        private string answer;
        private BitmapImage imageSource;
        private bool isCollapsed = true;
        private int questionIndex = 1;
        private double takeCount;

        public event EventHandler QuestionFinished;

        public event EventHandler TextBlockAnimationEvent;

        public PhotoQuestionViewModel(string filename, int index, MediaElement mediaElement)
        {
            this.filename = filename;
            QuestionIndex = index;
            random = new Random();
            this.mediaElement = mediaElement;
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
            if (answer == null)
            {
                await ShowQuestion();
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
                IsCollapsed = false;
                TextBlockAnimationEvent?.Invoke(this, EventArgs.Empty);
                using (var speechSynthesizer = new SpeechSynthesizer())
                {
                    var stream = await speechSynthesizer.SynthesizeTextToStreamAsync(answer);
                    mediaElement.SetSource(stream, stream.ContentType);
                    mediaElement.Play();
                }

                await Task.Delay(TimeSpan.FromSeconds(3));
                QuestionFinished?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                takeCount += 0.02;
                gameTimer.Start();
            }
        }

        private async Task ShowQuestion()
        {
            var width = 4;
            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    Blocks.Add(new BlockViewModel(width * j, width * i, width, width));
                }
            }

            Answer = QuestionsService.Instance.GetAnswer(filename);
            var file = await StorageFile.GetFileFromPathAsync(filename);
            var fileStream = await file.OpenAsync(FileAccessMode.Read);
            var img = new BitmapImage();
            img.SetSource(fileStream);

            ImageSource = img;
#if DEBUG
            takeCount = 50;
#else
            takeCount = 1;
#endif
        }
    }
}