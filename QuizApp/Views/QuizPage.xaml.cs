using QuizApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace QuizApp.Views
{
    public sealed partial class QuizPage : Page
    {
        public QuizPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
        }

        public QuizPageViewModel ViewModel => (DataContext as QuizPageViewModel);

        private async void PageOnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.Start();
        }

        private void PageOnUnloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.OnUnLoaded();
        }
    }
}