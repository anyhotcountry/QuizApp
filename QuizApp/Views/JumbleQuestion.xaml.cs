using QuizApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace QuizApp.Views
{
    public sealed partial class JumbleQuestion : UserControl
    {
        public JumbleQuestion()
        {
            InitializeComponent();
        }

        // strongly-typed view models enable x:bind
        public JumbleQuestionViewModel ViewModel => (DataContext as JumbleQuestionViewModel);
    }
}