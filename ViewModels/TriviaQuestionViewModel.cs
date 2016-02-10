using QuizApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace QuizApp.ViewModels
{
    public class TriviaQuestionViewModel : Mvvm.ViewModelBase, IQuestionViewModel
    {
        private readonly DispatcherTimer gameTimer;
        private readonly IMediaService mediaService;
        private readonly Random random;
        private readonly string filename;
        private readonly IQuestionsService questionsService;
        private string answer;
        private string question;
        private int questionIndex = 1;
        private double takeCount;
        private bool stopped;
        private double questionWidth;

        public event EventHandler QuestionFinished;

        public TriviaQuestionViewModel(string filename, int index, IQuestionsService questionsService, IMediaService mediaService)
        {
            this.filename = filename;
            QuestionIndex = index;
            random = new Random();
            this.mediaService = mediaService;
            this.questionsService = questionsService;
#if DEBUG
            gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
#else
            gameTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
#endif
            gameTimer.Tick += (s, e) => GameTimerOnTick();
            ShowQuestion();
            gameTimer.Start();
        }

        public string Answer
        {
            get { return answer; }

            set { Set(ref answer, value); }
        }

        public string Question
        {
            get { return question; }

            set { Set(ref question, value); }
        }

        public ObservableCollection<BlockViewModel> Blocks { get; } = new ObservableCollection<BlockViewModel>();

        public int QuestionIndex
        {
            get { return questionIndex; }

            set { Set(ref questionIndex, value); }
        }

        public double QuestionWidth
        {
            get { return questionWidth; }

            set { Set(ref questionWidth, value); }
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

        public void End()
        {
            Blocks.Clear();
        }

        private async void GameTimerOnTick()
        {
            gameTimer.Stop();
            if (stopped)
            {
                return;
            }

            if (Blocks.Count != 0)
            {
                var index = random.Next(Blocks.Count);
                Blocks.RemoveAt(index);
            }

            if (Blocks.Count == 0)
            {
                await mediaService.SpeakAsync(answer);
                await Task.Delay(TimeSpan.FromSeconds(3));
                QuestionFinished?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                gameTimer.Start();
            }
        }

        private async Task ShowQuestion()
        {
            var width = 10;
            for (int i = 0; i < 15; i++)
            {
                Blocks.Add(new BlockViewModel(width * i, 0, width, width));
            }

            Answer = questionsService.GetAnswer(filename);
            var file = await StorageFile.GetFileFromPathAsync(filename);
            Question = await FileIO.ReadTextAsync(file);
            await mediaService.SpeakAsync(Question);
            QuestionWidth = 2.5 * Question.Length;
        }
    }
}