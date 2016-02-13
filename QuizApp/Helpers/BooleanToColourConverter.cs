using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace QuizApp.Helpers
{
    public class BooleanToColourConverter : IValueConverter
    {
        public Color TrueColour { get; set; }

        public Color FalseColour { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = System.Convert.ToBoolean(value);
            return new SolidColorBrush(val ? TrueColour : FalseColour);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}