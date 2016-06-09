using System.Collections.ObjectModel;

namespace QuizApp.ViewModels
{
    public class ImageResultsViewModel : Mvvm.ViewModelBase
    {
        private string name;
        private ImageViewModel selectedItem;

        public string Name
        {
            get { return name; }
            set { Set(ref name, value); }
        }

        public ImageViewModel SelectedItem
        {
            get { return selectedItem; }
            set { Set(ref selectedItem, value); }
        }

        public ObservableCollection<ImageViewModel> Images { get; } = new ObservableCollection<ImageViewModel>();
    }
}