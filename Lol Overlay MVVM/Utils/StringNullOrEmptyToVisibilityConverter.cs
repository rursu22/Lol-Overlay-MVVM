using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Lol_Overlay_MVVM.Utils
{
    public class StringNullOrEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => string.IsNullOrEmpty(value as string)
               ? Visibility.Visible
               : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}