using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QuizApp.Helpers
{
    public sealed class DynamicDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            DataTemplate result = null;
            if (item == null)
            {
                return null;
            }

            string typeName = item.GetType().Name;

            try
            {
                result = App.Current.Resources[typeName + "Template"] as DataTemplate;
            }
            catch (Exception)
            {
                // No template that matches
            }

            return result;
        }
    }
}
