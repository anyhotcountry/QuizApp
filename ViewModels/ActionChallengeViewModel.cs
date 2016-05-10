using QuizApp.Services;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace QuizApp.ViewModels
{
    public class ActionChallengeViewModel : Mvvm.ViewModelBase, IQuestionViewModel
    {
        private readonly DispatcherTimer gameTimer;
        private readonly IMediaService mediaService;
        private readonly Random random;
        private readonly string filename;
        private readonly IQuestionsService questionsService;
        private string question;
        private int questionIndex = 1;
        private bool stopped;
        private double questionWidth;
        private int countDown;

        public event EventHandler QuestionFinished;

        public ActionChallengeViewModel(string filename, int index, IQuestionsService questionsService, IMediaService mediaService)
        {
            this.filename = filename;
            QuestionIndex = index;
            CountDown = 20;
            random = new Random();
            this.mediaService = mediaService;
            this.questionsService = questionsService;
            gameTimer = new DispatcherTimer();
            gameTimer.Tick += (s, e) => GameTimerOnTick();
            ShowQuestion();
        }

        public string Question
        {
            get { return question; }

            set { Set(ref question, value); }
        }

        public int CountDown
        {
            get { return countDown; }

            set { Set(ref countDown, value); }
        }

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
            CountDown = 1;
        }

        private async void GameTimerOnTick()
        {
            gameTimer.Stop();
            CountDown--;
            if (CountDown < 11 && CountDown >= 0)
            {
                await mediaService.SpeakAsync(CountDown.ToString());
            }

            if (CountDown < 0)
            {
                QuestionFinished?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                gameTimer.Start();
            }
        }

        private async Task ShowQuestion()
        {
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            var file = await StorageFile.GetFileFromPathAsync(filename);
            Question = await FileIO.ReadTextAsync(file);
            QuestionWidth = 3 * Question.Length;
            gameTimer.Start();
            await mediaService.SpeakAsync("Action Challenge: " + Question);
        }
    }
}