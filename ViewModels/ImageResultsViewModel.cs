using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;

namespace QuizApp.ViewModels
{
    public class ImageResultsViewModel : Mvvm.ViewModelBase
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { Set(ref name, value); }
        }

        public ObservableCollection<ImageSource> Images { get; } = new ObservableCollection<ImageSource>();
    }
}