using QuizApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace QuizApp.ViewModels
{
    public class PhotoQuestionViewModel : Mvvm.ViewModelBase, IQuestionViewModel
    {
        private readonly DispatcherTimer gameTimer;
        private readonly IMediaService mediaService;
        private readonly Random random;
        private readonly string filename;
        private readonly IQuestionsService questionsService;
        private string answer;
        private BitmapImage imageSource;
        private bool isCollapsed = true;
        private int questionIndex = 1;
        private double takeCount;
        private bool stopped;

        public event EventHandler QuestionFinished;

        public PhotoQuestionViewModel(string filename, int index, IQuestionsService questionsService, IMediaService mediaService)
        {
            this.filename = filename;
            QuestionIndex = index;
            random = new Random();
            this.mediaService = mediaService;
            this.questionsService = questionsService;
            gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            gameTimer.Tick += (s, e) => GameTimerOnTick();
            ShowQuestion();
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

        public void Stop()
        {
            stopped = true;
        }

        public void Start()
        {
            stopped = false;
            gameTimer.Start();
        }

        private async Task GameTimerOnTick()
        {
            gameTimer.Stop();
            if (stopped)
            {
                return;
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
                await mediaService.SpeakAsync(answer);

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

            Answer = questionsService.GetAnswer(filename);
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
