using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace YouthClubQuizApplication.ViewModels
{
    public class QuizPageViewModel : Mvvm.ViewModelBase
    {
        private static List<string> Questions = new List<string>
        {
            "Zoomed_Beaver.jpg",
            "Zoomed_Bobcat.jpg",
            "Zoomed_Chess.jpg",
            "Zoomed_Chicken.jpg",
            "Zoomed_Cork.jpg",
            "Zoomed_CreditCard.jpg",
            "Zoomed_Firefly.jpg",
            "Zoomed_FriedEgg.jpg",
            "Zoomed_Glasses.jpg",
            "Zoomed_Jalapeno.jpg",
            "Zoomed_LavaLamp.jpg",
            "Zoomed_Legos.jpg",
            "Zoomed_Megaphone.jpg",
            "Zoomed_Onion.jpg",
            "Zoomed_Oyster.jpg",
            "Zoomed_Plum.jpg",
            "Zoomed_Shoelace.jpg",
            "Zoomed_Shrimp.jpg",
            "Zoomed_Skunk.jpg",
            "Zoomed_Yam.jpg"
        };

        internal void SetSize(double width, double height)
        {
        }

        private readonly DispatcherTimer gameTimer;
        private readonly Random random;
        private string answer;
        private string imagePath;
        private int questionIndex;
        private double takeCount;

        public QuizPageViewModel()
        {
            random = new Random();
            gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            gameTimer.Tick += GameTimer_Tick;
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

        private async void GameTimer_Tick(object sender, object e)
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
                Answer = Questions[questionIndex].Replace("Zoomed_", string.Empty).Replace(".jpg", string.Empty);
                await Task.Delay(TimeSpan.FromSeconds(3));
            }

            if (Blocks.Count == 0 && questionIndex < Questions.Count)
            {
                questionIndex++;
                NextQuestion();
            }

            takeCount += 0.02;
            gameTimer.Start();
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

            ImagePath = "Images/" + Questions[questionIndex];
            Answer = string.Empty;
            takeCount = 1;
        }
    }
}

