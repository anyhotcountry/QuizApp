using QuizApp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace QuizApp.Views
{
    public sealed partial class QuizLauncherPage : Page
    {
        public QuizLauncherPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        public QuizLauncherPageViewModel ViewModel => (DataContext as QuizLauncherPageViewModel);
    }
}

