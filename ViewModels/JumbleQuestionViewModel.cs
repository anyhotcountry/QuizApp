using QuizApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace QuizApp.ViewModels
{
    public class JumbleQuestionViewModel : Mvvm.ViewModelBase, IQuestionViewModel
    {
        private readonly DispatcherTimer gameTimer;
        private readonly IMediaService mediaService;
        private readonly Random random;
        private readonly string filename;
        private readonly IQuestionsService questionsService;
        private readonly bool isPreview;
        private string answer;
        private int questionIndex = 1;
        private bool stopped;
        private IList<LetterViewModel> leftLetters;

        public event EventHandler QuestionFinished;

        public JumbleQuestionViewModel(string filename, int index, IQuestionsService questionsService, IMediaService mediaService, bool isPreview)
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

        public ObservableCollection<LetterViewModel> Letters { get; } = new ObservableCollection<LetterViewModel>();

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
                var curIndex = Letters.IndexOf(letter);
                var letterToMove = Letters[letter.Position];
                Letters[letter.Position] = letter;
                Letters[curIndex] = letterToMove;
                leftLetters.Remove(letter);
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
            answer = questionsService.GetAnswer(filename).ToUpper();
            gameTimer.Interval = TimeSpan.FromMilliseconds((isPreview ? 5000 : 30000) / (double)answer.Length);

            foreach (var letter in answer.Select((l, i) => new { Letter = l.ToString(), Position = i }).OrderBy(x => Guid.NewGuid()))
            {
                Letters.Add(new LetterViewModel { Letter = letter.Letter, Visible = false, Position = letter.Position });
            }

            leftLetters = new List<LetterViewModel>(Letters);
            var file = await StorageFile.GetFileFromPathAsync(filename);
            gameTimer.Start();
            await mediaService.SpeakAsync("Jumble word");
        }
    }
}