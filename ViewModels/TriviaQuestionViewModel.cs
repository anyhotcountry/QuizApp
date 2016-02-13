using QuizApp.Services;
using System;
using System.Collections.Generic;
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
        private bool stopped;
        private double questionWidth;
        private IList<LetterViewModel> leftLetters;

        public event EventHandler QuestionFinished;

        public TriviaQuestionViewModel(string filename, int index, IQuestionsService questionsService, IMediaService mediaService)
        {
            this.filename = filename;
            QuestionIndex = index;
            random = new Random();
            this.mediaService = mediaService;
            this.questionsService = questionsService;
            gameTimer = new DispatcherTimer();
            gameTimer.Tick += (s, e) => GameTimerOnTick();
            ShowQuestion();
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

        public ObservableCollection<LetterViewModel> Letters { get; } = new ObservableCollection<LetterViewModel>();

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
            foreach (var letter in leftLetters)
            {
                letter.Visible = true;
            }

            leftLetters.Clear();
        }

        private async void GameTimerOnTick()
        {
            gameTimer.Stop();
            if (stopped)
            {
                return;
            }

            if (leftLetters.Count != 0)
            {
                var index = random.Next(leftLetters.Count);
                leftLetters[index].Visible = true;
                leftLetters.RemoveAt(index);
            }

            if (leftLetters.Count == 0)
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
            Answer = questionsService.GetAnswer(filename);
#if DEBUG
            gameTimer.Interval = TimeSpan.FromMilliseconds(10000 / Answer.Length);
#else
            gameTimer.Interval = TimeSpan.FromMilliseconds(30000 / Answer.Length);
#endif

            foreach (var letter in Answer)
            {
                Letters.Add(new LetterViewModel { Letter = letter.ToString(), Visible = false });
            }

            leftLetters = new List<LetterViewModel>(Letters);
            var file = await StorageFile.GetFileFromPathAsync(filename);
            Question = await FileIO.ReadTextAsync(file);
            await mediaService.SpeakAsync(Question);
            QuestionWidth = 2.5 * Question.Length;
            gameTimer.Start();
        }
    }
}