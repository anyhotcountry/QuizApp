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
        private readonly bool isPreview;
        private string answer;
        private string question;
        private int questionIndex = 1;
        private bool stopped;
        private double questionWidth;
        private IList<LetterViewModel> leftLetters;

        public event EventHandler QuestionFinished;

        public TriviaQuestionViewModel(string filename, int index, IQuestionsService questionsService, IMediaService mediaService, bool isPreview)
        {
            this.filename = filename;
            this.isPreview = isPreview;
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
                var letter = leftLetters[index];
                letter.Visible = true;

                leftLetters.Remove(letter);
                Letters.Remove(letter);
                Letters.Insert(letter.Position, letter);
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
            Answer = questionsService.GetAnswer(filename).ToUpper();
            for (var i = 0; i < Answer.Length; i++)
            {
                Letters.Add(new LetterViewModel { Letter = Answer[i].ToString(), Visible = false, Position = i });
            }

            leftLetters = new List<LetterViewModel>(Letters);
            var file = await StorageFile.GetFileFromPathAsync(filename);
            Question = await FileIO.ReadTextAsync(file);
            QuestionWidth = 2.5 * Question.Length;
            gameTimer.Interval = TimeSpan.FromMilliseconds((isPreview ? 5000 : 30000) / (double)Answer.Length);
            gameTimer.Start();
            await mediaService.SpeakAsync(Question);
        }
    }
}