using System;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
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

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var folders = await ApplicationData.Current.LocalFolder.GetFoldersAsync(); ;
            var quizFolder = folders.FirstOrDefault(f => f.Name == "Quiz");
            if (folders.FirstOrDefault(f => f.Name == "Quiz") != null)
            {
                await quizFolder.DeleteAsync();
            }

            quizFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Quiz");
            var openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.Downloads;
            openPicker.FileTypeFilter.Add("*");
            var files = await openPicker.PickMultipleFilesAsync();
            foreach (var file in files)
            {
                await file.CopyAsync(quizFolder);
            }

            int mainViewId = ApplicationView.GetForCurrentView().Id;
            int? secondViewId = null;

            var view = CoreApplication.CreateNewView();
            await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                secondViewId = ApplicationView.GetForCurrentView().Id;
                var rootFrame = new Frame();
                rootFrame.Navigate(typeof(QuizPage), null);
                Window.Current.Content = rootFrame;
                Window.Current.Activate();
            });

            if (secondViewId.HasValue)
            {
                await ProjectionManager.StartProjectingAsync(secondViewId.Value, mainViewId);
            }

        }
    }
}

