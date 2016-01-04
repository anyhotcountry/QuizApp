using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QuizApp.Services
{
    public class PresentationService : IPresentationService
    {
        private readonly Type sourcePageType;

        public PresentationService(Type sourcePageType)
        {
            this.sourcePageType = sourcePageType;
        }

        public async Task ProjectAsync()
        {
            int mainViewId = ApplicationView.GetForCurrentView().Id;
            int? secondViewId = null;

            var view = CoreApplication.CreateNewView();
            await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                secondViewId = ApplicationView.GetForCurrentView().Id;
                var rootFrame = new Frame();
                rootFrame.Navigate(sourcePageType, null);
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
