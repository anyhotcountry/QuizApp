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
            QuizImage.SizeChanged += QuizImage_SizeChanged;
        }

        private void QuizImage_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            ViewModel.SetSize(e.NewSize.Width, e.NewSize.Height);
        }

        // strongly-typed view models enable x:bind
        public QuizPageViewModel ViewModel => this.DataContext as QuizPageViewModel;
    }
}
