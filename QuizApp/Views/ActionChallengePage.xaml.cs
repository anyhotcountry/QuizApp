using QuizApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace QuizApp.Views
{
    public sealed partial class ActionChallengePage : UserControl
    {
        public ActionChallengePage()
        {
            InitializeComponent();
        }

        // strongly-typed view models enable x:bind
        public ActionChallengeViewModel ViewModel => (DataContext as ActionChallengeViewModel);
    }
}