using System;
using System.Globalization;
using System.Windows.Data;

namespace Emma.XamlControls
{
    public class ParamsToSignatureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strings = (string[]) value;
            
            return strings == null 
                ? string.Empty 
                : string.Join(",", strings);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}