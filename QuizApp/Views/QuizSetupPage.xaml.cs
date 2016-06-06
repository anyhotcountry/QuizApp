using QuizApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace QuizApp.Views
{
    public sealed partial class QuizSetupPage : Page
    {
        public QuizSetupPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
        }

        // strongly-typed view models enable x:bind
        public QuizSetupPageViewModel ViewModel => this.DataContext as QuizSetupPageViewModel;
    }
}