using QuizApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace QuizApp.Views
{
    public sealed partial class PhotoQuestion : UserControl
    {
        public PhotoQuestion()
        {
            InitializeComponent();
        }

        public PhotoQuestionViewModel ViewModel => (DataContext as PhotoQuestionViewModel);

    }
}