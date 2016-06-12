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
        private readonly bool isPreview;
        private string question;
        private int questionIndex = 1;
        private bool stopped;
        private double questionWidth;
        private int countDown;
        private string message;
        private string answer;

        public event EventHandler QuestionFinished;

        public ActionChallengeViewModel(string filename, int index, IQuestionsService questionsService, IMediaService mediaService, bool isPreview)
        {
            this.filename = filename;
            this.isPreview = isPreview;
            QuestionIndex = index;
            countDown = isPreview ? 3 : 20;
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

        public string Message
        {
            get { return message; }

            set { Set(ref message, value); }
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
            countDown = 1;
        }

        private async void GameTimerOnTick()
        {
            gameTimer.Stop();
            countDown--;
            Message = countDown.ToString();
            if (countDown < 11 && countDown >= 0)
            {
                await mediaService.SpeakAsync(Message);
            }

            if (countDown < 0)
            {
                Message = answer;
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
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            var file = await StorageFile.GetFileFromPathAsync(filename);
            Question = await FileIO.ReadTextAsync(file);
            answer = questionsService.GetAnswer(filename);
            QuestionWidth = 3 * Question.Length;
            gameTimer.Start();
            await mediaService.SpeakAsync("Action Challenge: " + Question);
        }
    }
}