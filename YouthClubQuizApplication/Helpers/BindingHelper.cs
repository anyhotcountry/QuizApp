using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace QuizApp.Helpers
{
    public static class BindingHelper
    {
        public static readonly DependencyProperty CanvasLeftBindingPathProperty =
            DependencyProperty.RegisterAttached(
                "CanvasLeftBindingPath", typeof(string), typeof(BindingHelper),
                new PropertyMetadata(null, CanvasBindingPathPropertyChanged));

        public static readonly DependencyProperty CanvasTopBindingPathProperty =
            DependencyProperty.RegisterAttached(
                "CanvasTopBindingPath", typeof(string), typeof(BindingHelper),
                new PropertyMetadata(null, CanvasBindingPathPropertyChanged));

        public static string GetCanvasLeftBindingPath(DependencyObject obj)
        {
            return (string)obj.GetValue(CanvasLeftBindingPathProperty);
        }

        public static void SetCanvasLeftBindingPath(DependencyObject obj, string value)
        {
            obj.SetValue(CanvasLeftBindingPathProperty, value);
        }

        public static string GetCanvasTopBindingPath(DependencyObject obj)
        {
            return (string)obj.GetValue(CanvasTopBindingPathProperty);
        }

        public static void SetCanvasTopBindingPath(DependencyObject obj, string value)
        {
            obj.SetValue(CanvasTopBindingPathProperty, value);
        }

        private static void CanvasBindingPathPropertyChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var propertyPath = e.NewValue as string;

            if (propertyPath != null)
            {
                var CanvasProperty =
                    e.Property == CanvasLeftBindingPathProperty
                    ? Canvas.LeftProperty
                    : Canvas.TopProperty;

                BindingOperations.SetBinding(
                    obj,
                    CanvasProperty,
                    new Binding { Path = new PropertyPath(propertyPath) });
            }
        }
    }
}
