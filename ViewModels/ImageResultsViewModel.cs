using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;

namespace QuizApp.ViewModels
{
    public class ImageResultsViewModel : Mvvm.ViewModelBase
    {
        private string name;
        private ImageSource selectedItem;

        public string Name
        {
            get { return name; }
            set { Set(ref name, value); }
        }

        public ImageSource SelectedItem
        {
            get { return selectedItem; }
            set { Set(ref selectedItem, value); }
        }

        public ObservableCollection<ImageSource> Images { get; } = new ObservableCollection<ImageSource>();
    }
}