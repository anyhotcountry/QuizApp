using QuizApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace QuizApp.Views
{
    public sealed partial class TriviaQuestion : UserControl
    {
        public TriviaQuestion()
        {
            InitializeComponent();
        }

        // strongly-typed view models enable x:bind
        public TriviaQuestionViewModel ViewModel => (DataContext as TriviaQuestionViewModel);
    }
}
