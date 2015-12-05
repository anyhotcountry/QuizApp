using Windows.UI.Xaml.Controls;
using YouthClubQuizApplication.ViewModels;

namespace YouthClubQuizApplication.Views
{
    public sealed partial class QuizPage : Page
    {
        public QuizPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
        }

        // strongly-typed view models enable x:bind
        public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;
    }
}
