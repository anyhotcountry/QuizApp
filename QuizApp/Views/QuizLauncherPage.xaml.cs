using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
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

        async private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            int mainViewId = ApplicationView.GetForCurrentView().Id;
            int? secondViewId = null;

            var view = CoreApplication.CreateNewView();
            await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                secondViewId = ApplicationView.GetForCurrentView().Id;

                var rootFrame = new Frame();
                Window.Current.Content = rootFrame;
                rootFrame.Navigate(typeof(QuizPage), null);
            });

            if (secondViewId.HasValue)
            {
                await ProjectionManager.StartProjectingAsync(secondViewId.Value, mainViewId);
            }

        }
    }
}

