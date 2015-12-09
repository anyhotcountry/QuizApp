using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QuizApp.ViewModels
{
    public class QuizPageViewModel : Mvvm.ViewModelBase
    {
        private static List<string> Questions = new List<string>
        {
            "Zoomed_Fossil.jpg",
            "Zoomed_TeePee.jpg",
            "Zoomed_GoldChain.jpg",
            "Zoomed_Hawk.jpg",
            "Zoomed_Boar.jpg",
            "Zoomed_HorseShoe.jpg",
            "Zoomed_Nectarine.jpg",
            "Zoomed_Cotton.jpg",
            "Zoomed_BarbedWire.jpg",
            "Zoomed_Lipstick.jpg",
            "Zoomed_Chick.jpg",
            "Zoomed_Mango.jpg",
            "Zoomed_Shotgun.jpg",
            "Zoomed_Honeycomb.jpg",
            "Zoomed_Charcoal.jpg",
            "Zoomed_Flute.jpg",
            "Zoomed_Olives.jpg",
            "Zoomed_Handcuffs.jpg",
            "Zoomed_Pan.jpg",
            "Zoomed_Crab.jpg",
        };

        private readonly DispatcherTimer gameTimer;
        private readonly MediaElement mediaElement;
        private readonly Random random;
        private string answer;
        private string imagePath;
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
            NextQuestion();
        }

        public string Answer
        {
            get { return answer; }

            set { Set(ref answer, value); }
        }

        public ObservableCollection<BlockViewModel> Blocks { get; } = new ObservableCollection<BlockViewModel>();

        public string ImagePath
        {
            get { return imagePath; }

            set { Set(ref imagePath, value); }
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
                var answerRaw = Questions[questionIndex - 1].Replace("Zoomed_", string.Empty).Replace(".jpg", string.Empty);
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

        private void NextQuestion()
        {
            var width = 4;
            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    Blocks.Add(new BlockViewModel(width * j, width * i, width, width));
                }
            }

            ImagePath = "Images/" + Questions[questionIndex - 1];
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