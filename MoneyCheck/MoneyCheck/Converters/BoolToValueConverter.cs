using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MoneyCheck.Converters
{

    class BoolToColorConverter : BoolToValueConverter<Color> { }
    class BoolToTextConverter : BoolToValueConverter<String> { }
    class BoolToValueConverter<T> : IValueConverter
    {
        public T TrueValue { get; set; }
        public T FalseValue { get; set; }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? FalseValue : ((bool)value ? TrueValue : FalseValue);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null && EqualityComparer<T>.Default.Equals((T)value, TrueValue);
        }
    }
}
