using QuizApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace QuizApp.Views
{
    public sealed partial class AnswersPage : Page
    {
        public AnswersPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
        }

        // strongly-typed view models enable x:bind
        public AnswersPageViewModel ViewModel => this.DataContext as AnswersPageViewModel;
    }
}
