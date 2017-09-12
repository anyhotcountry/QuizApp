using QuizApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace QuizApp.Views
{
    public sealed partial class TriviaPhotoQuestion : UserControl
    {
        public TriviaPhotoQuestion()
        {
            InitializeComponent();
        }

        // strongly-typed view models enable x:bind
        public TriviaPhotoQuestionViewModel ViewModel => (DataContext as TriviaPhotoQuestionViewModel);
    }
}