using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;

namespace YouthClubQuizApplication.ViewModels
{
    public class QuizPageViewModel : Mvvm.ViewModelBase
    {
        private readonly DispatcherTimer gameTimer;
        private readonly Random random;
        private int takeCount;
        private int questionIndex;

        private static List<string> Questions = new List<string>
        {
            "Images/Zoomed_Beaver.jpg",
            "Images/Zoomed_Bobcat.jpg",
            "Images/Zoomed_Chess.jpg",
            "Images/Zoomed_Chicken.jpg",
            "Images/Zoomed_Cork.jpg",
            "Images/Zoomed_CreditCard.jpg",
            "Images/Zoomed_Firefly.jpg",
            "Images/Zoomed_FriedEgg.jpg",
            "Images/Zoomed_Glasses.jpg",
            "Images/Zoomed_Jalapeno.jpg",
            "Images/Zoomed_LavaLamp.jpg",
            "Images/Zoomed_Legos.jpg",
            "Images/Zoomed_Megaphone.jpg",
            "Images/Zoomed_Onion.jpg",
            "Images/Zoomed_Oyster.jpg",
            "Images/Zoomed_Plum.jpg",
            "Images/Zoomed_Shoelace.jpg",
            "Images/Zoomed_Shrimp.jpg",
            "Images/Zoomed_Skunk.jpg",
            "Images/Zoomed_Yam.jpg"
    };

        public QuizPageViewModel()
        {
            random = new Random();
            gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
            NextQuestion();
        }

        private void NextQuestion()
        {
            var width = 34;
            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 22; j++)
                {
                    Blocks.Add(new BlockViewModel(283 + width * j, width * i, width, width));
                }
            }

            ImagePath = Questions[questionIndex];
            takeCount = 1;
        }

        private void GameTimer_Tick(object sender, object e)
        {
            for (int i = 0; i < takeCount; i++)
            {
                if (Blocks.Count != 0)
                {
                    var index = random.Next(Blocks.Count);
                    Blocks.RemoveAt(index);
                }
                else if (questionIndex < Questions.Count)
                {
                    questionIndex++;
                    NextQuestion();
                }
            }

            takeCount += (Blocks.Count == 200 || Blocks.Count == 300) ? 1 : 0;
        }



        public ObservableCollection<BlockViewModel> Blocks { get; } = new ObservableCollection<BlockViewModel>();

        private string imagePath = "Images/Zoomed_Beaver.jpg";

        public string ImagePath
        {
            get { return imagePath; }

            set { Set(ref imagePath, value); }
        }
    }
}

