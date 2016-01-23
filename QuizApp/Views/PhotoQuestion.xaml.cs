using Windows.UI.Xaml.Controls;

namespace QuizApp.Views
{
    public sealed partial class PhotoQuestion : UserControl
    {
        public PhotoQuestion()
        {
            InitializeComponent();
            QuizImage.SizeChanged += (o, e) =>
            {
                BlocksViewBox.Width = QuizImage.ActualWidth;
                BlocksViewBox.Height = QuizImage.ActualHeight;
            };
        }
    }
}