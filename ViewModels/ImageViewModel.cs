using System;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace QuizApp.ViewModels
{
    public class ImageViewModel
    {
        private ImageSource imageSource;
        public Uri Uri { get; set; }

        public ImageSource ImageSource
        {
            get { return imageSource = imageSource ?? new BitmapImage(Uri); }
        }
    }
}