using QuizApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace QuizApp.Views
{
    public sealed partial class PrintAnswersPage : Page
    {
        public PrintAnswersPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
        }

        // strongly-typed view models enable x:bind
        public PrintAnswersPageViewModel ViewModel => this.DataContext as PrintAnswersPageViewModel;
    }
}
