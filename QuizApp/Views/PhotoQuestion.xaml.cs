using QuizApp.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace QuizApp.Views
{
    public sealed partial class PhotoQuestion : UserControl
    {
        public PhotoQuestion()
        {
            InitializeComponent();
            DataContextChanged += (sender, e) =>
            {
                ViewModel.TextBlockAnimationEvent += Start_Animation;
            };
        }

        // strongly-typed view models enable x:bind
        public PhotoQuestionViewModel ViewModel => (DataContext as PhotoQuestionViewModel);

        private async void Start_Animation(object sender, EventArgs e)
        {
            await Task.Delay(100);
            var board = new Storyboard();
            var timeline = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(timeline, rotateTransform);
            Storyboard.SetTargetProperty(timeline, "Angle");
            var frame = new EasingDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromSeconds(1),
                Value = 360,
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            timeline.KeyFrames.Add(frame);
            timeline.AutoReverse = true;
            board.Children.Add(timeline);

            board.Begin();
        }
    }
}